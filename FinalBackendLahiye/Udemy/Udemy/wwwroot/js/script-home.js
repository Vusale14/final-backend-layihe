const initSlider = () => {
    const container = document.querySelector(".slider .container");
    const slideButtons = document.querySelectorAll(".slider .slide-button");
    const sliderScrollbar = document.querySelector(".slider .slider-scrollbar");
    const scrollbarThumb = sliderScrollbar.querySelector(".scrollbar-thumb");
    const maxScrollLeft = container.scrollWidth - container.clientWidth;

    scrollbarThumb.addEventListener("mousedown", (e) => {
        const startX = e.clientX;
        const thumbPosition = scrollbarThumb.offsetLeft;

        const handleMouseMove = (e) => {
            const deltaX = e.clientX - startX;
            const newThumbPosition = thumbPosition + deltaX;
            const maxThumbPosition = sliderScrollbar.getBoundingClientRect().width - scrollbarThumb.offsetWidth;

            const boundedPosition = Math.max(0, Math.min(maxThumbPosition, newThumbPosition));
            const scrollPosition = (boundedPosition / maxThumbPosition) * maxScrollLeft;

            scrollbarThumb.style.left = `${boundedPosition}px`;
            container.scrollLeft = scrollPosition;
        }

        const handleMouseUp = () => {
            document.removeEventListener("mousemove", handleMouseMove);
            document.removeEventListener("mouseup", handleMouseUp);
        }

        document.addEventListener("mousemove", handleMouseMove);
        document.addEventListener("mouseup", handleMouseUp);
    });

    slideButtons.forEach(button => {
        button.addEventListener("click", () => {
            const direction = button.id === "prev-slide" ? -1 : 1;
            const scrollAmount = container.clientWidth * direction;
            container.scrollBy({ left: scrollAmount, behavior: "smooth" });
        });
    });

    const handleSlideButtons = () => {
        slideButtons[0].style.display = container.scrollLeft <= 0 ? "none" : "block";
        slideButtons[1].style.display = container.scrollLeft >= maxScrollLeft ? "none" : "block";
    }

    const updateScrollThumbPosition = () => {
        const scrollPosition = container.scrollLeft;
        const thumbPosition = (scrollPosition / maxScrollLeft) * (sliderScrollbar.clientWidth - scrollbarThumb.offsetWidth);
        scrollbarThumb.style.left = `${thumbPosition}px`;
    }

    container.addEventListener("scroll", () => {
        handleSlideButtons();
        updateScrollThumbPosition();
    });
}

window.addEventListener("load", initSlider);

const initCourseSlider = () => {
    const container = document.querySelector(".courses-table .courses");
    const slideButtons = document.querySelectorAll(".courses-table .slide-button");

    slideButtons.forEach(button => {
        button.addEventListener("click", () => {
            const direction = button.id === "prev-slide" ? -1 : 1;
            const scrollAmount = container.clientWidth * direction;
            container.scrollBy({ left: scrollAmount, behavior: "smooth" });
        });
    });
}

window.addEventListener("load", initCourseSlider);