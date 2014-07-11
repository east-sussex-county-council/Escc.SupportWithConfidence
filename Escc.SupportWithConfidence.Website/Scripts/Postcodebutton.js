$(function(){
    $("#ctl00_content_results_ctl01_txbPostcode").keydown(function (event) {
	
if (event.which == 13)
	{

	event.preventDefault();
	$("#ctl00_content_results_ctl01_btnSearch").click();

	
	}
});

});
