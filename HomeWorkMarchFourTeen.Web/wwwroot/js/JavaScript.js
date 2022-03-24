$(() => {
    $("#new-contributor").on('click', function () {
        console.log("hello!")
        $('[name="FirstName"]').val('');
        $('[name="LastName"]').val('');
        $('[name="Cell"]').val('');
        
        $('[name="Date"]').val('');
        $("#initialDepositDiv").show();
        $(".new-contrib").modal();
        $("#Edit").text('New Contributor');
    });
    $(".deposit-button").on('click', function () {
        const Name = $(this).data('first-name');
        $('#deposit-name').text(Name);
        console.log(Name);
        const Id = $(this).data('contrib-id');
        $('[name="contributorId"]').val(Id);
        console.log(Id);
        $(".deposit").modal();
    });
    $(".edit-contrib").on('click', function () {
        const FirstName = $(this).data('first-name');
        $('[name="FirstName"]').val(FirstName);

        const LastName = $(this).data('last-name');
        $('[name="LastName"]').val(LastName);

        const Cell = $(this).data('cell');
        $('[name="Cell"]').val(Cell);

        const Id = $(this).data('id');

        const AlwaysInclude = $(this).data('always-include');
        $('[name="ShouldAlwaysBeIncluded"]').prop("checked", AlwaysInclude === "True");

        const Date = $(this).data('date');
        $('[name="Date"]').val(Date);

        $(".modal-body").append(`<input type="hidden" name="Id" value="${Id}"/>`);

        $("#initialDepositDiv").hide();

        $(".new-contrib").modal();

        $(".auction").attr('action', "/contributors/edit");

        $("#Edit").text('Edit');
    });
});