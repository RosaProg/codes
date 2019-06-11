$(function () {
    skinChanger();
    activateNotificationAndTasksScroll();
    setSkinListHeightAndScroll();
    setSettingListHeightAndScroll();
    $(window).resize(function () {
        setSkinListHeightAndScroll();
        setSettingListHeightAndScroll();
    });
    
    // Save Program
    $("#save_program").click(function(){
        console.log("Save Program");
        console.log("Course Name " + $("#course_name").val());
        console.log("Course Description " + $("#course_description").val());
        console.log("Course Start Date " + $("#course_start_date").val());
        console.log("Course End Date " + $("#course_end_date").val());
        console.log("Course Cost " + $("#course_cost").val());
        console.log("Course Pass Mark " + $("#course_pass_mark").val());
        console.log("Course Venue " + $("#venue").val());
        console.log("Course Pretest Enabled " + $("#pretest_enabled").is(':checked'));
        console.log("Course Exams Enabled " + $("#exams_enabled").is(':checked'));
        console.log("Course Employee Segment " + $("#employee_segment").val());
        console.log("Course Employee Level " + $("#employee_level").val());
        console.log("Course Cliente " + $("#course_client").val());
        
        $.post("../../api/index.php",
        {
            'q':'save_program',
            'course_name':$("#course_name").val(),
            'course_description':$("#course_description").val(),
            'course_start_date':$("#course_start_date").val(),
            'course_end_date':$("#course_end_date").val(),
            'course_cost':$("#course_cost").val(),
            'course_pass_mark':$("#course_pass_mark").val(),
            'venue':$("#venue").val(),
            'pretest_enabled':$("#pretest_enabled").is(':checked'),
            'exams_enabled':$("#exams_enabled").is(':checked'),
            'employee_segment':$("#employee_segment").val(),
            'employee_level':$("#employee_level").val(),
            'course_client':$("#course_client").val()
        }, function( data ) {
            console.log(JSON.stringify(data));
            if(data != false){
                $("#save_program_successfull").show(0).delay(5000).hide(0);
            }else{
                $('#save_program_error').show(0).delay(5000).hide(0);
            }
        });
        
    });
    
    // Save Exam
    $("#save_question").click(function(){
        console.log("Save Exam");
        console.log("Program ID " + $("#program_id").val());
        console.log("Test Title " + $("#exam_title").val());
        console.log("Test Start Date " + $("#exam_start_date").val());
        console.log("Test End Date " + $("#exam_end_date").val());
        console.log("Module ID " + $("#exam_module").val());
        console.log("Question " + $("#exam_question").val());
        console.log("Answer 1 " + $("#answer_1").val());
        console.log("Answer 2 " + $("#answer_2").val());
        console.log("Answer 3 " + $("#answer_3").val());
        console.log("Answer 4 " + $("#answer_4").val());
        console.log("Answer 5 " + $("#answer_5").val());
        console.log("Correct Answer " + $("#correct_answer").val());
        
        $.post("../../api/index.php",
        {
            'q':'save_exam',
            'course_id':$("#program_id").val(),
            'exam_title':$("#exam_title").val(),
            'exam_start_date':$("#exam_start_date").val(),
            'exam_end_date':$("#exam_end_date").val(),
            'course_unit':$("#exam_module").val(),
            'question':$("#exam_question").val(),
            'answer1':$("#answer_1").val(),
            'answer2':$("#answer_2").val(),
            'answer3':$("#answer_3").val(),
            'answer4':$("#answer_4").val(),
            'answer5':$("#answer_5").val(),
            'correct_answer':$("#correct_answer").val()
        }, function( data ) {
            console.log(JSON.stringify(data));
            if(data != false){
                $("#exam_question").val("");
                $("#answer_1").val("");
                $("#answer_2").val("");
                $("#answer_3").val("");
                $("#answer_4").val("");
                $("#answer_5").val("");
                $("#correct_answer").val(0);
                $("#save_exam_successfull").show(0).delay(5000).hide(0);
            }else{
                $('#save_exam_error').show(0).delay(5000).hide(0);
            }
        });
        
    });
    
    //Get Trainers Data
    if(document.getElementById('programs_body') || document.getElementById('programs_combo')){
        console.log("ENTRO!!!");
        $.post("../../api/index.php",
        {
            'q':'programs'
        }, function( data ) {
            console.log(JSON.stringify(data));
            if(data != false){
                data = JSON.parse(data);
                if(document.getElementById('programs_body')){
                    var colors = ['pink', 'cyan', 'teal', 'orange', 'purple'];
                    for(var i=0;i<data.length;i++)
                    {
                        var color = colors[i % colors.length];
                        document.getElementById('programs_body').innerHTML+='<tr class="bg-'+color+'">'+
                        '<td>' + data[i].course_start_date + '</td>' +
                        '<td>' + data[i].course_end_date + '</td>' +
                        '<td>' + data[i].course_description + '</td>' +
                        '<td>' + data[i].venue + '</td>' +
                        '<td>' + data[i].rating + '</td>' +
                        '<td>' + data[i].preview_image + '</td>' +
                        '<td>' + data[i].course_complete + '</td>' +
                        '<td>' + data[i].name + '</td>' +
                        '</tr>';
                    }
                }
                if(document.getElementById('programs_combo')){
                    console.log("ENTRO!!!!!!!");
                    for(var i=0;i<data.length;i++)
                    {
                        console.log('<option value="'+data[i].id+'">'+data[i].name+'</option>');
                        $('#programs_combo').append('<option value="'+data[i].id+'">'+data[i].course_name+'</option>');
                    }
                }
            }else{
                //console.log(JSON.stringify(data));
            }
        });
    }
    
    
    //Get Attendance Data
    $.post("../../api/index.php",
    {
        'q':'attendance'
    }, function( data ) {
        console.log(JSON.stringify(data));
        if(data != false){
            data = JSON.parse(data);
            var colors = ['active', 'success', 'info', 'warning', 'danger'];
            for(var i=0;i<data.length;i++)
            {
                var color = colors[i % colors.length];
                document.getElementById('attendance').innerHTML+='<tr class="'+color+'">'+
                '<td>' + data[i].user_id + '</td>' +
                '<td>' + data[i].user_name + '</td>' +
                '<td>' + data[i].program_id + '</td>' +
                '<td>' + data[i].program_name + '</td>' +
                '<td>' + data[i].datetime + '</td>' +
                '<td>' + data[i].action + '</td>' +
                '</tr>';
            }
        }else{
            console.log(JSON.stringify(data));
        }
    });
    
    //Get Trainers Data
    $.post("../../api/index.php",
    {
        'q':'trainers'
    }, function( data ) {
        console.log(JSON.stringify(data));
        if(data != false){
            data = JSON.parse(data);
            for(var i=0;i<data.length;i++)
            {
                document.getElementById('trainers_body').innerHTML+='<tr>'+
                '<td>' + data[i].name + '</td>' +
                '<td>' + data[i].username + '</td>' +
                '<td>' + data[i].address + ', ' + data[i].city + ', ' + data[i].state + ', ' + data[i].country + '</td>' +
                '<td>' + data[i].phone + '</td>' +
                '<td>' + data[i].creation_date + '</td>' +
                '<td>' + data[i].rating + '</td>' +
                '</tr>';
            }
        }else{
            console.log(JSON.stringify(data));
            //$('#login_failed').show();
        }
    });
    
    // Search
    $("#search").keyup(function() {
        var value = this.value;
        $("table").find("tr").each(function(index) {
            if (index === 0) return;
            var if_td_has = false; //boolean value to track if td had the entered key
            $(this).find('td').each(function () {
                if_td_has = if_td_has || $(this).text().indexOf(value) !== -1; //Check if td's text matches key and then use OR to check it for all td's
            });
            $(this).toggle(if_td_has);
        });
    });
    
    // --- UPLOADER PPT PDF ---- //
    // Change this to the location of your server-side upload handler:
    var url = window.location.hostname === 'blueimp.github.io' ?
                '//jquery-file-upload.appspot.com/' : '../../server/php/';
    
    console.log(url);
    $('#cor_file').fileupload({
        url: url,
        dataType: 'json',
        done: function (e, data) {
            $.each(data.result.files, function (index, file) {
                console.log(file.name);
                $('<p/>').html('<a href="' + url + 'files/'+file.name+'">' + file.name + '</a>').appendTo('#files1');
            });
        },
        progressall: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css('width', progress + '%');
            //$('#progress .progress-bar').removeClass('active');
        }
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');
    
});

//Skin changer
function skinChanger() {
    $('.right-sidebar .demo-choose-skin li').on('click', function () {
        var $body = $('body');
        var $this = $(this);

        var existTheme = $('.right-sidebar .demo-choose-skin li.active').data('theme');
        $('.right-sidebar .demo-choose-skin li').removeClass('active');
        $body.removeClass('theme-' + existTheme);
        $this.addClass('active');

        $body.addClass('theme-' + $this.data('theme'));
    });
}

//Skin tab content set height and show scroll
function setSkinListHeightAndScroll() {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.demo-choose-skin');

    $el.slimScroll({ destroy: true }).height('auto');
    $el.parent().find('.slimScrollBar, .slimScrollRail').remove();

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Setting tab content set height and show scroll
function setSettingListHeightAndScroll() {
    var height = $(window).height() - ($('.navbar').innerHeight() + $('.right-sidebar .nav-tabs').outerHeight());
    var $el = $('.right-sidebar .demo-settings');

    $el.slimScroll({ destroy: true }).height('auto');
    $el.parent().find('.slimScrollBar, .slimScrollRail').remove();

    $el.slimscroll({
        height: height + 'px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Activate notification and task dropdown on top right menu
function activateNotificationAndTasksScroll() {
    $('.navbar-right .dropdown-menu .body .menu').slimscroll({
        height: '254px',
        color: 'rgba(0,0,0,0.5)',
        size: '4px',
        alwaysVisible: false,
        borderRadius: '0',
        railBorderRadius: '0'
    });
}

//Feedback ===============================================================================================
window.doorbellOptions = {
    appKey: 'vKPuEJ1myGKeT7Vrim0st1HSQJM9YjR6co9rH1sD9XxsTrOScnTy5qokHSetSJeA'
};
(function(w, d, t) {
    var hasLoaded = false;
    function l() { if (hasLoaded) { return; } hasLoaded = true; window.doorbellOptions.windowLoaded = true; var g = d.createElement(t);g.id = 'doorbellScript';g.type = 'text/javascript';g.async = true;g.src = 'https://embed.doorbell.io/button/4832?t='+(new Date().getTime());(d.getElementsByTagName('head')[0]||d.getElementsByTagName('body')[0]).appendChild(g); }
    if (w.attachEvent) { w.attachEvent('onload', l); } else if (w.addEventListener) { w.addEventListener('load', l, false); } else { l(); }
    if (d.readyState == 'complete') { l(); }
}(window, document, 'script'));

//Google Analiytics ======================================================================================
/*addLoadEvent(loadTracking);
var trackingId = 'UA-30038099-6';

function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
}

function loadTracking() {
    (function (i, s, o, g, r, a, m) {
        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
            (i[r].q = i[r].q || []).push(arguments)
        }, i[r].l = 1 * new Date(); a = s.createElement(o),
        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
    })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

    ga('create', trackingId, 'auto');
    ga('send', 'pageview');
}*/
//========================================================================================================