(function () {
    "use strict";
    document.addEventListener('DOMContentLoaded', function () {
        tinymce.init({ selector: 'textarea', menubar: false, statusbar: false, plugins: "link", toolbar: "undo redo | bold link" });
    })
})();
