const sidebarOpenBtnElement = document.querySelector(
    ".navbar > .fa-bars"
);

const sidebarCloseBtnElement = document.querySelector(
    ".navbar > .sidebar > .fa-solid"
);

const sidebarElement = document.querySelector(
    ".navbar .sidebar"
);

sidebarElement.addEventListener("click",(e) => {
    e.stopPropagation();
});

sidebarOpenBtnElement.addEventListener("click", (e) => {
    e.stopPropagation();
    sidebarElement.classList.add("open");
});

sidebarCloseBtnElement.addEventListener("click", () => {
    sidebarElement.classList.remove("open");
});

window.onclick = () => {
    sidebarElement.classList.remove("open");
};