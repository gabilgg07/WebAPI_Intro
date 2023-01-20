function loadGroups() {
  let data = null;

  $.ajax({
    type: "GET",
    url: "http://aaliyeva0791-001-site1.itempurl.com/api/groups",
    // data: "data",
    // dataType: "dataType",
    success: function (response) {
      data = response;
    },
    async: false,
  });

  return data;
}

$(function () {
  let data = loadGroups();

  $(data).each(function (index, element) {
    let tr = $(`<tr>
    <th scope="row">${element.id}</th>
    <td>${element.name}</td>
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
      //   success: function (response) {
      //     console.log("success");
      //     $("#createGroup").modal("hide");
      //     window.location.reload();
      //   },
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
});
