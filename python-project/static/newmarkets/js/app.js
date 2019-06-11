$( document ).ready(function() {



    // ========================================================================================================== Show Onboarding
    $("#show_onboarding").click(function(){
        $("#onboarding_wrap").show(0);
    })
    $("#cancel_onboarding").click(function(){
        $("#onboarding_wrap").hide(0);
    })



    
    
    
    if(jQuery.support.touch){
        $("#sidebar").click( function() { // on mousedown
            $("#sidebar").css("width","225px");
            $("#body-row").css("left","225px");
        });
        $("#body-row").click( function() { // on mousedown
            $("#sidebar").css("width","55px");
            $("#body-row").css("left","55px");
        });
    }else{
        //alert('not touch enabled');
    }
        
    
    
    
    
    
    //Hover body, sidebar disappears
    $( "#body-row" ).hover(function() {
        $(".hidden-settings").slideUp();
    });


    //  Click username
    $( "#user-actions" ).click(function() {
        if($(".hidden-settings").is(':visible')) {
            $(".hidden-settings").slideUp();
        }
        else {
            $(".hidden-settings").slideDown();
        }

    });








});
