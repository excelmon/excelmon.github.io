/* Shared behavior for phillipberger.com — used by / and /projects/ */

// ── Theme (persisted across pages) ──
function applyTheme(theme) {
  var html = document.documentElement;
  var btn  = document.getElementById('themeBtn');
  if (theme === 'light') {
    html.setAttribute('data-theme', 'light');
    if (btn) btn.textContent = '[ dark ]';
  } else {
    html.removeAttribute('data-theme');
    if (btn) btn.textContent = '[ light ]';
  }
}

function toggleTheme() {
  var next = document.documentElement.getAttribute('data-theme') === 'light' ? 'dark' : 'light';
  applyTheme(next);
  try { localStorage.setItem('site-theme', next); } catch (e) {}
}

// Sync the button label with whatever the <head> snippet already applied
applyTheme(document.documentElement.getAttribute('data-theme') === 'light' ? 'light' : 'dark');

// ── Mobile folder-tab nav ──
function toggleFolderNav(btn) {
  btn.classList.toggle('open');
  document.getElementById('folderDropdown').classList.toggle('open');
}

function closeFolderNav() {
  var drop = document.getElementById('folderDropdown');
  var btn  = document.getElementById('folderTabBtn');
  if (drop) drop.classList.remove('open');
  if (btn)  btn.classList.remove('open');
}

// Close folder dropdown if clicking outside
document.addEventListener('click', function(e) {
  var wrap = document.querySelector('.folder-nav-wrap');
  if (wrap && !wrap.contains(e.target)) closeFolderNav();
});

// Sync tab button width to dropdown, then draw SVG border
function drawTabBorder() {
  var btn = document.getElementById('folderTabBtn');
  var drop = document.getElementById('folderDropdown');
  if (!btn || !drop) return;

  // Measure dropdown width
  drop.style.visibility = 'hidden';
  drop.style.display = 'block';
  var dropW = drop.offsetWidth;
  drop.style.display = '';
  drop.style.visibility = '';

  btn.style.width = dropW + 'px';

  var poly = document.getElementById('tabBorderPoly');
  if (!poly) return;
  var w = btn.offsetWidth;
  var h = btn.offsetHeight;
  var notch = 9;
  var pts = [
    [0, h],
    [0, 0],
    [w - notch, 0],
    [w, notch],
    [w, h]
  ].map(function(p){ return p[0]+','+p[1]; }).join(' ');
  poly.setAttribute('points', pts);
}

window.addEventListener('load', drawTabBorder);
window.addEventListener('resize', drawTabBorder);
setTimeout(drawTabBorder, 100);
