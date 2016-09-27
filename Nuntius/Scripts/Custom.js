function CountVote(id, userId) {
    var result = "";
    $.ajax({
        method: 'Post',
        url: '/Article/CountVote',
        async: false,
        data: {
            id: id,
            userId: userId
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            result=data;

        }
    });
    return result;
};
function CommentUpvote(comment) {

    $.ajax({
        method: 'Post',
        url: '/Article/CommentUpVote',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            ShowComments();
        }
    });
};
function CommentDownvote(comment) {

    $.ajax({
        method: 'Post',
        url: '/Article/CommentDownVote',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            ShowComments();
        }
    });
};
function deleteComment(comment) {
    $.ajax({
        method: 'Post',
        url: '/Article/DeleteComment',
        data: {
            id: comment.getAttribute("data-commentId"),
            userId: comment.getAttribute("data-userId")
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            if(data == "Deleted")
            $(comment).closest("li").remove();
        }
    });
}
function ShowComments() {
    $.ajax({
        method: 'Post',
        url: '/Article/ShowComments',
        data: {
            url: $('#url').val(),
        },
        dataType: 'json',
        error: function (jqXHR, textStatus, errorThrown) {
            alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
        },
        success: function (data) {
            if (data === undefined) {
                alert('no data send');
            }
            else {
                $('#comments-list').empty();
                for (var i = 0; i < data.length; i++) {
                    var divcontent0 = CountVote(data[i].CommentId, data[i].ApplicationUserId);

                    var divcontent1 = '<li><div class="comment-main-level"><!-- Avatar --><div class="comment-avatar"><img src=' + data[i].User.ProfilePicture;
                    var divcontent2 = '></div><div class="comment-box"><div class="comment-head"><h6 class="comment-name by-author"><a href="#">' + data[i].User.UserName + '</a></h6>';
                    var divcontent3 = '<span>' + data[i].DatePublished + '</span>' +
                        '<i onclick="deleteComment(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-remove"></i> ' +
                        '<i onclick="CommentDownvote(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-arrow-down"></i> ' +
                        '<i>'+divcontent0 + '</i>'+
                        '<i onclick="CommentUpvote(this)" data-userId = ' + data[i].ApplicationUserId + ' data-commentId=' + data[i].CommentId + ' class="glyphicon glyphicon-arrow-up"></i> ' +
                        '</div><div class="comment-content">' +
                        data[i].CommentText + '</div></div></div></li>';
                    $('#comments-list').append(divcontent1 + divcontent2 + divcontent3);
                }


            }
            //$('#test').html("Letade du efter " + data.Name + "? id=" + data.Id);
        }
    });
}
$(document).ready(function () {
    $('#SendComment').on('click', function () {
        
        $.ajax({
            method: 'Post',
            url: '/Article/AddComment',
            data: {
                comment: $('#message-text').val(),
                urlId: $('#urlId').val(),
                articleSource: $('#articleSource').val()
            },
            dataType: 'json',
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
            },
            success: function (data) {
                $('#message-text').val('');
                $('#showAlert').html( '<div class="alert alert-info">'+
                    '<a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>'+
                data+'</div>');
                ShowComments();
            }
        });
    });//on input
    //$('#ShowComments').on('click', function () {
    
    window.onload = ShowComments;


    //});




//    $(document).on('click', '#deleteComment', function () {

//        $.ajax({
//            method: 'Post',
//            url: '/Article/DeleteComment',
//            data: {
//                id: $('#comments-list').change(function () {
//                    $(this).find(':selected').data("commentId");
//                })
//            },
//            dataType: 'json',
//            error: function (jqXHR, textStatus, errorThrown) {
//                alert('Något gick fel! status:' + textStatus + "\nerror: " + errorThrown);
//            },
//            success: function (data) {
//                if (data.Name === undefined)
//                    $('#test').html(data);
//                else
//                    $('#test').html("Letade du efter " + data.Name + "? id=" + data.Id);
//            }
//        });
//    });//on input
    });