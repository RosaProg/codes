//Accordian
$('.accordion').each(function () {
    var $accordian = $(this);
    $accordian.find('.accordion-head').on('click', function () {
        $accordian.find('.accordion-body').slideUp();
        $accordian.find('span').css("backgroundPosition", '0px 0px');
        $accordian.find('.resultCircle-bar').show(300);
        if (!$(this).next().is(':visible')) {
            $(this).next().slideDown();
            $(this).find('.resultCircle-bar').hide(300);
            $(this).find('span').css({ "background-position": '20px 0px' });
        }
    });
});

function Toggle(id) {
   
}
