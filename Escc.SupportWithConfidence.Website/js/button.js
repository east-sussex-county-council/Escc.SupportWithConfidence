$(function(){
    $("#1_SearchControls_ctl03_txbProvider").keydown(function (event) {
	
if (event.which == 13)
	{

	event.preventDefault(); 
    $("#1_SearchControls_ctl03_btnSearch").click();

	
	}
});

});
