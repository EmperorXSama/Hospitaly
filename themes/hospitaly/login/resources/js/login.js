(function () {
  'use strict';

  /* ─── Mouse tracking for gradient overlay ─── */

  var root = document.documentElement;
  var rafId = null;
  var mouseX = 50;
  var mouseY = 50;

  function updatePosition(e) {
    mouseX = (e.clientX / window.innerWidth) * 100;
    mouseY = (e.clientY / window.innerHeight) * 100;
    if (!rafId) {
      rafId = requestAnimationFrame(applyPosition);
    }
  }

  function applyPosition() {
    rafId = null;
    root.style.setProperty('--mouse-x', mouseX + '%');
    root.style.setProperty('--mouse-y', mouseY + '%');
  }

  document.addEventListener('mousemove', updatePosition, { passive: true });

  /* ─── Floating Particles (animejs) ─── */

  var particlesContainer = document.getElementById('loginParticles');
  var animeLib = window.anime;

  if (particlesContainer && animeLib) {
    var animate = typeof animeLib === 'function' ? animeLib : (animeLib.animate || animeLib);

    if (typeof animate !== 'function') {
      console.warn('[login] animejs animate not found, falling back to CSS animation');
      particlesContainer.classList.add('particles-css-fallback');
    } else {
      var pastelColors = [
        'rgba(252, 228, 236, 0.35)',
        'rgba(232, 224, 240, 0.35)',
        'rgba(220, 232, 245, 0.35)',
        'rgba(189, 187, 255, 0.2)',
        'rgba(239, 44, 193, 0.06)'
      ];

      function createParticle() {
        var el = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        el.setAttribute('viewBox', '0 0 100 100');
        el.style.position = 'absolute';
        el.style.pointerEvents = 'none';
        el.style.overflow = 'visible';

        var size = 20 + Math.random() * 50;
        var color = pastelColors[Math.floor(Math.random() * pastelColors.length)];

        var circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        circle.setAttribute('cx', '50');
        circle.setAttribute('cy', '50');
        circle.setAttribute('r', '50');
        circle.setAttribute('fill', color);
        el.appendChild(circle);

        var startX = 5 + Math.random() * 90;
        var startY = 100 + Math.random() * 30;
        var duration = 12000 + Math.random() * 18000;
        var drift = -30 + Math.random() * 60;

        el.style.left = startX + '%';
        el.style.top = startY + '%';
        el.style.width = size + 'px';
        el.style.height = size + 'px';
        el.style.transform = 'scale(0.4)';
        el.style.opacity = '0';

        particlesContainer.appendChild(el);

        animate({
          targets: el,
          opacity: [{ value: 0.8, duration: duration * 0.15 }, { value: 0, duration: duration * 0.15, delay: duration * 0.7 }],
          scale: [{ value: 1.2, duration: duration * 0.5 }, { value: 0.4, duration: duration * 0.3 }],
          translateY: -(100 + Math.random() * 60) + '%',
          translateX: drift + '%',
          duration: duration,
          easing: 'linear',
          loop: true,
          delay: Math.random() * duration
        });
      }

      for (var i = 0; i < 18; i++) {
        createParticle();
      }
    }
  } else if (particlesContainer) {
    particlesContainer.classList.add('particles-css-fallback');
  }

  /* ─── SVG Cursor Circle Ripple ─── */

  var cursorCircle = document.getElementById('cursorCircle');
  if (cursorCircle) {
    var circ = cursorCircle.querySelector('circle');

    function moveCursorCircle(e) {
      cursorCircle.style.left = (e.clientX - 50) + 'px';
      cursorCircle.style.top = (e.clientY - 50) + 'px';
    }

    document.addEventListener('mousemove', moveCursorCircle, { passive: true });

    document.addEventListener('click', function (e) {
      if (e.target.closest('.btn, .form-input, .social-provider-btn, a'))
        return;
      cursorCircle.style.left = (e.clientX - 50) + 'px';
      cursorCircle.style.top = (e.clientY - 50) + 'px';
      circ.setAttribute('r', '0');
      circ.setAttribute('opacity', '1');

      if (window.anime) {
        var animFn = typeof window.anime === 'function' ? window.anime : (window.anime.animate || null);
        if (typeof animFn === 'function') {
          animFn({
            targets: circ,
            r: [0, 50],
            opacity: [0.8, 0],
            duration: 600,
            easing: 'easeOutQuad',
            complete: function () {
              circ.setAttribute('r', '0');
              circ.setAttribute('opacity', '1');
            }
          });
          return;
        }
      }

      /* CSS fallback for ripple */
      circ.setAttribute('r', '50');
      circ.setAttribute('opacity', '0');
      setTimeout(function () {
        circ.setAttribute('r', '0');
        circ.setAttribute('opacity', '1');
      }, 600);
    });
  }

})();
