$(function () {
$('<a href="#" id="showall" class="expandall">Expand all sub-categories below</a>').insertBefore("ul#navigation").click(function () {
    if ($(this).hasClass("expandall"))
	{
	$("a.expand").click();
	$(this).text("Collapse all sub-categories below").removeClass("expandall").addClass("collapseall");
	}
	else
	{
	$("a.collapse").click();
	$(this).text("Expand all sub-categories below").removeClass("collapseall").addClass("expandall");
	}
});

var first = true;
$("ul#navigation li:has(ul)").append('<a href="#" class="collapse">Collapse</a>').click(function () {
    if ($("a.collapse", this).length === 1) {
        if (first) {
            $("ul", this).hide();
        }
        else {
            $("ul", this).slideUp();
        }

        $("a.collapse", this).text("Expand").attr("class", "expand");
    }
    else {
        $("ul", this).slideDown();
        $("a.expand", this).text("Collapse").attr("class", "collapse");
    }
});

$("a.collapse").click();
first = false;

});
