document.addEventListener("DOMContentLoaded", function () {
    var scrollToForm = '@(TempData["scrollToForm"] != null ? "true" : "false")';

    if (scrollToForm === "true") {
        var formSection = document.getElementById("form-section");
        if (formSection) {
            formSection.scrollIntoView({ behavior: "smooth" });
        }
    }
});

