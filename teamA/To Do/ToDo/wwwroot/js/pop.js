$(function () {

    var time = 700;
    var $aside = $('#container #side-bar');
    var $sideButton = $aside.find('button');
    var butonText = $sideButton.text();

    $sideButton.on('click', function () {
        $aside.toggleClass('open');
        if ($aside.hasClass('open')) {
            $aside.stop(true).animate({
                left: 0
            }, time, 'easeOutSine');
            $sideButton.text(butonText.replace("open...", "close..."));
        } else {
            $aside.stop(true).animate({
                left: '-250px'
            }, time, 'easeOutSine');
            $sideButton.text(butonText.replace("close...", "open..."));
        }
    });
});