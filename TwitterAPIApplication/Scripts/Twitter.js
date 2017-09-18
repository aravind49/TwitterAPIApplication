$(document).ready(function () {

    $("#txt_Search").keyup(function () {
        var txt = $("#txt_Search").val().trim();
        if (txt.length > 0) {
            $("#div_twitterView li").each(function () {               
                var tweetText = $(this).find(".bubble").text().toLowerCase();
                if (tweetText.indexOf(txt.toLowerCase()) > -1) {
                    $(this).css('display', 'block');
                }
                else {
                    $(this).css('display', 'none');
                }
            });            
        }
        else {
            $("#div_twitterView li").css('display', 'block');
        }

    })

    LoadTwitterPage();
    RefreshTwitterPage();

});

function RefreshTwitterPage() {
    setTimeout(function () {
        LoadTwitterPage();
        RefreshTwitterPage();
    }, 60000);
}

function LoadTwitterPage() {
  
    $.ajax({
        url: '/Home/Twitter',
        type: 'get',
        cache: false,
        async: true,
        success: function (result) {
            $('#div_twitterView').html(result);
        }
    });

}