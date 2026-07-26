/* Luxury Line CRM — Client-side interactivity */
(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {

        /* ---- Active nav link ---- */
        var links = document.querySelectorAll('.navbar-nav .nav-link');
        var currentPage = window.location.pathname.split('/').pop().toLowerCase();
        links.forEach(function (link) {
            var href = (link.getAttribute('href') || '').split('/').pop().toLowerCase();
            if (href && href === currentPage) {
                link.classList.add('active');
            }
        });

        /* ---- Enter key triggers Search button in the same card ---- */
        document.querySelectorAll('.card input[type="text"]').forEach(function (input) {
            var card = input.closest('.card');
            if (!card) return;
            /* look for a visible search button inside the card */
            var btn = card.querySelector(
                'input[type="submit"][id*="Search"], button[type="submit"][id*="Search"], ' +
                'input[type="submit"][id*="search"], button[type="submit"][id*="search"]'
            );
            if (!btn) return;
            input.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    btn.click();
                }
            });
        });

        /* ---- Bootstrap tooltips ---- */
        if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
            document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
                new bootstrap.Tooltip(el);
            });
        }

        /* ---- Auto-dismiss success alerts after 4 s ---- */
        function fadeOutAlert(el) {
            el.style.transition = 'opacity 0.5s';
            el.style.opacity = '0';
            el.addEventListener('transitionend', function () { el.style.display = 'none'; }, { once: true });
        }
        document.querySelectorAll('.alert-success').forEach(function (el) {
            setTimeout(fadeOutAlert, 4000, el);
        });

    });
})();
