(function ($) {
  $.fn.getFormData = function (addProperties) {
    let temp = this;
    let obj = {};

    $(temp)
      .find("input[name]")
      .each(function (index, item) {
        // if (
        //   item.value.length > 0 &&
        //   !isNaN(item.value) &&
        //   $(item).attr("name") == "id"
        // ) {
        //   obj[item.name] = parseInt(item.value);
        //   return;
        // }

        obj[item.name] = item.value;
      });

    obj = $.extend(obj, addProperties);

    return obj;
  };
})(jQuery);
