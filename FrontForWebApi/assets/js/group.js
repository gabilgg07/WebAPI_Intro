function loadGroups() {
  let data = null;

  $.ajax({
    type: "GET",
    url: "http://aaliyeva0791-001-site1.itempurl.com/api/groups",
    success: function (response) {
      data = response;
    },
    async: false,
  });

  return data;
}
function getGroup(id) {
  let data = null;

  $.ajax({
    type: "GET",
    url: `http://aaliyeva0791-001-site1.itempurl.com/api/groups/${id}`,
    success: function (response) {
      data = response;
    },
    async: false,
  });

  return data;
}

$(function () {
  let data = loadGroups();

  let count = 1;

  $(data).each(function (i, element) {
    let tr = $(`<tr>
    <th scope="row">${count++}</th>
    <td>${element.name}</td>
    <td class="d-flex justify-content-end" data-item-id="${element.id}">
        <a class="btn btn-outline-warning me-2 editItem"><i class="icon-edit"></i></a>
        <a class="btn btn-outline-danger delItem"><i class="icon-delete"></i></a>
    </td>
  </tr>`);
    $("#groupTbl tbody").append(tr);
  });

  $("#btnAdd").click(function (e) {
    e.preventDefault();

    $("#createGroup")[0].reset();

    let modal = $("#createGroup").modal({
      backdrop: "static",
      keyboard: true,
      show: true,
    });

    $(modal).modal("show");
  });

  $("#createGroup").submit(function (e) {
    e.preventDefault();
    let group = $(e.currentTarget).getFormData();

    $.ajax({
      type: "post",
      url: "http://aaliyeva0791-001-site1.itempurl.com/api/groups",
      data: JSON.stringify(group),
      contentType: "application/json",
      statusCode: {
        200: function (response) {
          $("#createGroup").modal("hide");
          window.location.reload();
        },
        201: function (response) {
          $("#createGroup").modal("hide");
          window.location.reload();
        },
        404: function (response) {
          console.log("response: ");
          console.log(response);
        },
      },
    });
  });

  $(".delItem").click(function (e) {
    e.preventDefault();
    e.stopPropagation();

    let id = $(e.currentTarget).parent().data("itemId");

    if (confirm("Are you sure, for to delete?")) {
      $.ajax({
        type: "DELETE",
        url: `http://aaliyeva0791-001-site1.itempurl.com/api/groups/${id}`,
        data: "data",
        dataType: "dataType",
        statusCode: {
          204: function (response) {
            alert("silindi!");
            window.location.reload();
          },
          404: function (response) {
            alert("tapilmadi!");
          },
        },
        error: function (response) {
          console.log("error:  ");
          console.log(response);
        },
      });
    }
  });

  $(".editItem").click(function (e) {
    e.stopPropagation();

    let id = $(e.currentTarget).parent().data("itemId");

    $("#editGroup")[0].reset();

    let modal = $("#editGroup").modal({
      backdrop: "static",
      keyboard: true,
      show: true,
    });

    let group = getGroup(id);

    $(modal).find("[name=id]").val(group.id);
    $(modal).find("[name=name]").val(group.name);

    $(modal).modal("show");
  });

  $("#editGroup").submit(function (e) {
    e.preventDefault();

    let group = $(e.currentTarget).getFormData();
    group.id = parseInt(group.id);

    $.ajax({
      type: "put",
      url: `http://aaliyeva0791-001-site1.itempurl.com/api/groups/${group.id}`,
      data: JSON.stringify(group),
      contentType: "application/json",
      success: function (response) {
        $("#createGroup").modal("hide");
        window.location.reload();
      },
      error: function (response) {
        console.log("response: ");
        console.log(response);
      },
    });
  });
});
