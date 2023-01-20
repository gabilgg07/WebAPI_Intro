(function ($) {
  $.fn.getFormData = function (addProperties) {
    let temp = this;
    let obj = {};

    $(temp)
      .find("input[name]")
      .each(function (index, item) {
        obj[item.name] = item.value;
      });

    obj = $.extend(obj, addProperties);

    return obj;
  };
})(jQuery);
