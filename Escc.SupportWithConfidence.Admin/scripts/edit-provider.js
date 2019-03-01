(function () {
    "use strict";
    document.addEventListener("DOMContentLoaded", function () {

        // Wire up remove category buttons to remove the list item surrounding the button, which also includes the hidden field
        let removeButtons = document.getElementsByClassName("remove-category");
        Array.prototype.forEach.call(removeButtons, function (button) {
            button.addEventListener("click", function (e) {
                e.preventDefault();
                this.parentNode.parentNode.removeChild(this.parentNode);
            });
        });

        // Wire up an autocomplete of categories using JQuery UI, which is already present in the page as part of the sitewide template
        if (typeof (jQuery) !== 'undefined') {
            let autocompleteTarget = jQuery("#select-category");
            if (autocompleteTarget.autocomplete) {
                autocompleteTarget.autocomplete({
                    source: autocompleteTarget.data("autocomplete-url"),
                    minLength: 2,
                    select: function (e, ui) {
                        let template = $($.trim($("#category-template").html()));
                        $("p,span", template).append(ui.item.value);
                        $("input", template).val(ui.item.id);
                        template.appendTo(".edit-categories");
                        $(this).val('');
                        return false;
                    }
                });
            }
        }
    });
})();