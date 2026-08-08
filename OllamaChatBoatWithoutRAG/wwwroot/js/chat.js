$(document).ready(function () {

    // Send button click
    $("#btnSend").click(function () {
        alert("Clicked!");
        sendMessage();
    });

    // Press Enter to send
    $("#txtMessage").keypress(function (e) {
        if (e.which === 13) {
            e.preventDefault();
            sendMessage();
       }
    });

});

function sendMessage() {

    let message = $("#txtMessage").val().trim();

    if (message === "")
        return;

    // Add user message
    appendUserMessage(message);

    $.post("/Home/Send",
        {
            conversationId:
                $("#conversationId").val(),
            message:
                $("#txtMessage").val()
          },

        function (result) {
            // appendAssistantMessage(result.response);
            var html = marked.parse(result.response);

            $("#messages").append(`
                    <div class="assistant-message mb-3">
                            ${html}
                    </div>
                `);

            hljs.highlightAll();
            var panel = $("#messages");
            panel.scrollTop(panel[0].scrollHeight);

        });

                

    // Clear textbox
    $("#txtMessage").val("");

    // Focus textbox
    $("#txtMessage").focus();
      

}

function appendUserMessage(message) {

    let html = `
        <div class="d-flex justify-content-end mb-3">

            <div class="card bg-primary text-white shadow"
                 style="max-width:70%;">

                <div class="card-body p-2">

                    <div>${escapeHtml(message)}</div>

                    <small class="text-light">
                        ${getCurrentTime()}
                    </small>

                </div>

            </div>

        </div>
    `;

    $("#messages").append(html);

    scrollToBottom();
}

function appendAssistantMessage(message) {

    let html = `
        <div class="d-flex justify-content-start mb-3">

            <div class="card bg-light shadow"
                 style="max-width:70%;">

                <div class="card-body p-2">

                    <strong>AI</strong>

                    <hr class="mt-1 mb-2"/>

                    <div>${escapeHtml(message)}</div>

                    <small class="text-muted">
                        ${getCurrentTime()}
                    </small>

                </div>

            </div>

        </div>
    `;

    $("#messages").append(html);

    scrollToBottom();
}

function scrollToBottom() {

    let panel = $("#messages");

    panel.scrollTop(panel.prop("scrollHeight"));
}

function getCurrentTime() {

    let now = new Date();

    return now.toLocaleTimeString([], {
        hour: '2-digit',
        minute: '2-digit'
    });
}

function escapeHtml(text) {

    return $("<div/>").text(text).html();
}