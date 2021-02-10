// загрузка изображения-постера фильма
$('#addBannerBtn').on('click', function(e) {
    e.preventDefault();
    $('#BannerImage').trigger('click');
});

// отображение названия изображения
$('#BannerImage').on('change', function() {
    if($('#BannerImage')[0].files.length != 0) {
        var file = $('#BannerImage')[0].files[0];
        $('#bannerName').text(file['name']);
    }
});

// отправка данных формы на сервер с последующим 
// обновлением данных таблицы
$('.submitBtn').on('click', function(e) {
    e.preventDefault();
    var form = $('#movieForm'),  
        formData = new FormData(form[0]);
    $.validator.unobtrusive.parse(form);

    if ($(form).valid())
    {
        $('#movieModalDialog .modal-content')
            .html('<h4 class="text-center">Сохранение данных...</h4>');

        $.ajax({
            url: form.attr('action'), 
            type: form.attr('method'), 
            data: formData, 
            processData: false, 
            contentType: false
        }).then(function () {
            $('#movieModalDialog').modal('hide');
            $('#movieTable').dataTable().trigger('deselect');
            $('#movieTable').DataTable().ajax.reload();
        }).fail(function() {
            console.log('Что-то пошло не так.');
        });
    }        
});