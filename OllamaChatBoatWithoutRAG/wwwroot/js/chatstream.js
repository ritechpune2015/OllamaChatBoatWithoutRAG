let streamController = null;

marked.setOptions({
    breaks: true,
    gfm: true
});


$(function () {

    $("#btnSend").click(async function () {

        await sendMessage();

    });


    $("#txtMessage").keydown(async function (event) {

        if (event.key === "Enter" && !event.shiftKey) {

            event.preventDefault();

            await sendMessage();

        }

    });


    $("#btnStop").click(function () {

        stopGeneration();

    });


    async function sendMessage() {

        const input = $("#txtMessage");

        const message = input.val().trim();

        const conversationId =
            $("#conversationId").val();


        if (!message) {

            return;
        }


        // ---------------------------------------------
        // Display user message
        // ---------------------------------------------

        addUserMessage(message);


        // ---------------------------------------------
        // Clear input
        // ---------------------------------------------

        input.val("");

        input.focus();


        // ---------------------------------------------
        // Create assistant message
        // ---------------------------------------------

        const assistantElement =
            addAssistantMessage();


        // ---------------------------------------------
        // Disable Send
        // ---------------------------------------------

        $("#btnSend")
            .prop("disabled", true);


        // ---------------------------------------------
        // Enable Stop
        // ---------------------------------------------

        $("#btnStop")
            .prop("disabled", false);


        // ---------------------------------------------
        // Create AbortController
        // ---------------------------------------------

        streamController =
            new AbortController();


        // ---------------------------------------------
        // Store complete Markdown
        // ---------------------------------------------

        let fullResponse = "";


        try {

            // -----------------------------------------
            // Call streaming API
            // -----------------------------------------

            const response =
                await fetch("/Chat/Stream", {

                    method: "POST",

                    headers: {
                        "Content-Type":
                            "application/json"
                    },

                    body: JSON.stringify({

                        conversationId:
                            conversationId,

                        message:
                            message

                    }),

                    signal:
                        streamController.signal
                });


            if (!response.ok) {

                throw new Error(
                    `HTTP Error ${response.status}`
                );
            }


            if (!response.body) {

                throw new Error(
                    "Streaming is not supported."
                );
            }


            // -----------------------------------------
            // Get stream reader
            // -----------------------------------------

            const reader =
                response.body.getReader();


            // -----------------------------------------
            // UTF-8 decoder
            // -----------------------------------------

            const decoder =
                new TextDecoder("utf-8");


            // -----------------------------------------
            // Read stream
            // -----------------------------------------

            while (true) {

                const result =
                    await reader.read();


                if (result.done) {

                    break;
                }


                const chunk =
                    decoder.decode(
                        result.value,
                        {
                            stream: true
                        });


                // -------------------------------------
                // Accumulate Markdown
                // -------------------------------------

                fullResponse += chunk;


                // -------------------------------------
                // Display raw text during streaming
                // -------------------------------------

                assistantElement
                    .textContent = fullResponse;


                // -------------------------------------
                // Auto scroll
                // -------------------------------------

                scrollToBottom();
            }


            // -----------------------------------------
            // Flush decoder
            // -----------------------------------------

            const finalChunk =
                decoder.decode();

            if (finalChunk) {

                fullResponse += finalChunk;
            }


            // -----------------------------------------
            // Render final Markdown
            // -----------------------------------------

            renderMarkdown(
                assistantElement,
                fullResponse
            );


            // -----------------------------------------
            // Auto scroll
            // -----------------------------------------

            scrollToBottom();

        }
        catch (error) {

            if (error.name === "AbortError") {

                console.log(
                    "Generation stopped by user."
                );

                return;
            }


            console.error(error);

            assistantElement.textContent =
                "Sorry, something went wrong while generating the response.";
        }
        finally {

            $("#btnSend")
                .prop("disabled", false);


            $("#btnStop")
                .prop("disabled", true);


            streamController = null;

            input.focus();
        }
    }


    // =====================================================
    // Render Markdown
    // =====================================================

    function renderMarkdown(
        element,
        markdown) {

        const rawHtml =
            marked.parse(markdown);


        const safeHtml =
            DOMPurify.sanitize(rawHtml);


        element.innerHTML =
            safeHtml;


        highlightCode(element);

        addCopyButtons(element);
    }


    // =====================================================
    // Syntax Highlighting
    // =====================================================

    function highlightCode(element) {

        const codeBlocks =
            element.querySelectorAll(
                "pre code"
            );


        codeBlocks.forEach(
            function (block) {

                hljs.highlightElement(
                    block
                );

            });
    }


    // =====================================================
    // Copy Code Buttons
    // =====================================================

    function addCopyButtons(element) {

        const codeBlocks =
            element.querySelectorAll(
                "pre"
            );


        codeBlocks.forEach(
            function (pre) {

                // Avoid duplicate buttons

                if (pre.querySelector(
                    ".copy-code-button")) {

                    return;
                }


                const button =
                    document.createElement(
                        "button"
                    );


                button.className =
                    "copy-code-button";


                button.textContent =
                    "Copy";


                button.addEventListener(
                    "click",
                    async function () {

                        const code =
                            pre.querySelector(
                                "code"
                            );


                        if (!code) {

                            return;
                        }


                        await navigator.clipboard
                            .writeText(
                                code.innerText
                            );


                        button.textContent =
                            "Copied!";


                        setTimeout(
                            function () {

                                button.textContent =
                                    "Copy";

                            },
                            1500
                        );
                    }
                );


                pre.appendChild(button);

            });
    }


    // =====================================================
    // Add User Message
    // =====================================================

    function addUserMessage(message) {

        const container =
            document.createElement("div");


        container.className =
            "user-message-container";


        const messageElement =
            document.createElement("div");


        messageElement.className =
            "user-message";


        // IMPORTANT:
        // textContent prevents HTML injection

        messageElement.textContent =
            message;


        container.appendChild(
            messageElement
        );


        $("#messages").append(
            container
        );


        scrollToBottom();
    }


    // =====================================================
    // Add Assistant Message
    // =====================================================

    function addAssistantMessage() {

        const container =
            document.createElement("div");


        container.className =
            "assistant-message-container";


        const messageElement =
            document.createElement("div");


        messageElement.className =
            "assistant-message";


        container.appendChild(
            messageElement
        );


        $("#messages").append(
            container
        );


        scrollToBottom();


        return messageElement;
    }


    // =====================================================
    // Stop Generation
    // =====================================================

    function stopGeneration() {

        if (streamController) {

            streamController.abort();

            streamController = null;
        }


        $("#btnStop")
            .prop("disabled", true);


        $("#btnSend")
            .prop("disabled", false);
    }


    // =====================================================
    // Auto Scroll
    // =====================================================

    function scrollToBottom() {

        const panel =
            $("#messages")[0];


        if (!panel) {

            return;
        }


        panel.scrollTop =
            panel.scrollHeight;
    }

});
