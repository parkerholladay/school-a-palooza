function handleToggleAccordion(event) {
    var toggleTitle = event.target;
    var toggleId = toggleTitle.id.replace('-header', '-toggle');
    var toggle = document.getElementById(toggleId);

    if (!$(toggle).attr('class').includes('collapsed')) {
        $(toggleTitle).removeClass('open');
    } else {
        $(toggleTitle).addClass('open');
    }

    $.each($('.accordion-panel-title'), (i, e) => {
        if ($(e).attr('class').includes('open') && e.id !== toggleTitle.id) {
            $(e).removeClass('open');
        }
    });
}
