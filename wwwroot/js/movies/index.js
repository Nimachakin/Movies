
$(document).ready(function() {
    if($('#movieTable').length != 0) {
        var dataTable = initDataTable($('#movieTable')); 
        applyTableSelectHandler(dataTable);
    }
});

// вызываем модальное окно с формой редактирования/просмотра данных 
// о фильме
$('#crud a').on('click', function(e) {
    e.preventDefault(); 
    
    $('#movieModalDialog .modal-content')
        .html('<h4 class="text-center">Подготовка формы...</h4>');
    $('#movieModalDialog').modal('show');

    $.ajax({
        url: $(this).attr('href')
    }).then(function(content) {
        $('#movieModalDialog .modal-content').html(content);
    }).fail(function() {
        console.log('Что-то пошло не так.');
    });
});

// инициализируем таблицу с данными о фильмах, 
// хранящихся в базе данных
function initDataTable($table) {
    var dataTable = $table.dataTable({
        ajax: { 
            url: '/Movie/GetAll', 
            dataSrc: "", 
        },  
        language: {
            url: '/json/dataTable_ru.json'
        },                   
        processing: true, 
        select: { style: 'single' },                      
        columns:
        [
            { data: 'name', title: 'Наименование' }, 
            { data: 'year', title: 'Год выпуска' }, 
            { data: 'director', title: 'Режиссер' }, 
            { data: 'userName', title: 'Запись опубликована:' }
        ]
    });

    dataTable.fnSort([0, 'asc']);
    return dataTable;
}

// добавляем обработчики выбора (снятия выбора) записи из таблицы данных
function applyTableSelectHandler(dataTable) {
    // при выборе записи таблицы активируем кнопки редактирования и просмотра
    dataTable.DataTable().on('select', function (e, dt, type, indexes) {
        var movieId = dt.rows(indexes).data().pluck('id')[0]; // идентификатор фильма
        var postOwner = dt.rows(indexes).data().pluck('userName')[0]; // пользователь, создавший запись о фильме 
        var currentUser = dataTable.data('user'); // текущий пользователь

        $('#crud a:not(.createBtn)').each(function () { 
            // не активируем кнопку редактирования, 
            // если текущий пользователь не является автором выбранной записи 
            if($(this).hasClass('editBtn') && postOwner != currentUser)
            {
                return;
            }

            $(this).removeClass('disabled');
            $(this).attr('href', function(i, val) {                        
                return val.concat('?id=', movieId);
            });
        });
    });

    // при снятии выбора записи таблицы деактивируем кнопки редактирования и просмотра
    dataTable.DataTable().on('deselect', function (e, dt, type, indexes) {
        $('#crud a:not(.createBtn)').each(function () { 
            $(this).addClass('disabled');
            $(this).attr('href', function(i, val) { 
                return val.split('?')[0];
            });
        })
    });
}        