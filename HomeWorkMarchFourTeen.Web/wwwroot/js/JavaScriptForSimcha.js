$(() => {
    $("#new-simcha").on('click', function () {
        console.log("hello!")
        $(".dialog-box").modal();
    });
});