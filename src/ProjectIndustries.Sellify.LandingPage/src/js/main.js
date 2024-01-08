(function () {
  var slideout = new Slideout({
    'panel': document.querySelector('.Root'),
    'menu': document.querySelector('.MobileMenu'),
    'padding': 256,
    'tolerance': 70
  });

  var mainNav = document.querySelector(".MainNav");
  slideout.on("open", tryToggleSlideout);
  slideout.on("close", tryToggleSlideout);
// Toggle button
  document.querySelector('.MobileMenuToggle').addEventListener('click', function () {
    slideout.toggle();
    tryToggleSlideout();
  });


  new SmoothScroll('a[href*="#"]', {
    speed: 500,
    header: '[data-scroll-header]'
  });

  window.addEventListener("scroll", function () {
    mainNav.classList.toggle("is-scrolled", !!window.scrollY);
  });

  function tryToggleSlideout() {
    mainNav.classList.toggle("is-menuOpened", slideout.isOpen());
  }

  window.addEventListener("load", function () {
    AOS.init();
    var formMembers = document.getElementsByClassName("SubscribeForm-member")
    var form = document.querySelector(".SubscribeForm");
    var alertEl = document.querySelector(".Alert");
    var alertTitle = alertEl.querySelector(".Alert-title");
    var alertDesc = alertEl.querySelector(".Alert-desc");
    form.addEventListener("submit", function (e) {
      e.preventDefault();
      resetAlert();
      toggleSubscribeFormElements(false);
      signUp(form, function (data) {
        toggleSubscribeFormElements(true);
        if (data.result !== "error") {
          form.parentElement.removeChild(form);
          showAlert("Thank you!", "Your email was added to the mailing list!", false);
        } else {
          showAlert("Error", data.msg, true);
        }
      });
    });

    function signUp(form, callback) {
      var additionalParams = [];
      for (var i = 0; i < form.elements.length; i++) {
        var el = form.elements[i];
        let param = el.name;
        if (el.value) {
          param +=  "=" + encodeURIComponent(el.value);
        }

        additionalParams.push(param);
      }

      loadJSONP(form.action + "&" + additionalParams.join("&"), callback);
    }

    function showAlert(title, message, isError) {
      resetAlert();
      alertEl.classList.add("is-visible");
      alertEl.classList.add(isError ? "Alert--error" : "Alert--success");
      alertTitle.innerText = title;
      alertDesc.innerHTML = message;
    }

    function resetAlert() {
      alertEl.classList.remove("is-visible");
      alertEl.classList.remove("Alert--error");
      alertEl.classList.remove("Alert--success");
    }

    function toggleSubscribeFormElements(enable) {
      for (var i = 0; i < formMembers.length; i++) {
        var member = formMembers[i];
        enable
          ? member.removeAttribute("disabled")
          : member.setAttribute("disabled", "disabled");
      }
    }
  });
})();

var loadJSONP = (function(){
  var unique = 0;
  return function(url, callback, context) {
    // INIT
    var name = "_jsonp_" + unique++;
    if (url.match(/\?/)) url += "&c="+name;
    else url += "?c="+name;

    // Create script
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = url;

    // Setup handler
    window[name] = function(data){
      callback.call((context || window), data);
      document.getElementsByTagName('head')[0].removeChild(script);
      script = null;
      delete window[name];
    };

    // Load JSON
    document.getElementsByTagName('head')[0].appendChild(script);
  };
})();

