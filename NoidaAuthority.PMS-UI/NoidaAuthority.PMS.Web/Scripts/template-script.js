
$(document).ready(function(){

	/* Mobile menu */
	$('.mobile-menu-icon').click(function(){
	    $('.noidaauth-left-nav').slideToggle();
	});

	/* Close the widget when clicked on close button */
	$('.noidaauth-content-widget .fa-times').click(function () {
		$(this).parent().slideUp(function(){
			$(this).hide();
		});
	});
});