
window.onload = function () {
    Hotbuys();
    Recommended();
    Culinary();
};
//  Hot Buys, Low Prices! 
function Hotbuys() {
    const container = document.querySelector('#hblp-elements');
    const elements = document.querySelectorAll('#hblp-element');
    const NextBtn = document.querySelector("#hnlpNext");
    const PrevBtn = document.querySelector("#hnlpPrev");
    let current_page = 0;
    let end_page = elements.length - 5;
    console.log(elements.length);
    const elementWidth = container.offsetWidth / elements.length;
    function showPage(page) {
        const offset = page * elementWidth;
        container.style.transform = `translateX(-${offset}px)`;
        container.style.transition = 'transform 0.5s ease-in-out';
        if (elements.length <= 5) {

            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'none';
        }
        else if (page >= end_page || end_page <= 0) {
            NextBtn.style.display = 'none'
            PrevBtn.style.display = 'block';
        }

        else if (page == 0) {
            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'block';
        }
        else {
            NextBtn.style.display = 'block';
            PrevBtn.style.display = 'block';
        }
    }

    document.querySelector("#hnlpNext").addEventListener('click', () => {
        if (current_page <= end_page) {
            current_page++;
            showPage(current_page);
        }
        if (current_page > 1) {
            PrevBtn.style.display = 'block';

        }
        console.log("Next: " + current_page);
    });

    document.querySelector("#hnlpPrev").addEventListener('click', () => {
        if (current_page > 0) {
            current_page--;
            showPage(current_page);
        }
        console.log("Prev: " + current_page);

    });

    showPage(current_page);
}
// Recommended

function Recommended() {
    const container = document.querySelector('#rfu-elements');
    const elements = document.querySelectorAll('#rfu-element');
    const NextBtn = document.querySelector("#rfuNext");
    const PrevBtn = document.querySelector("#rfuPrev");
    let current_page = 0;
    let end_page = elements.length - 5;
    const elementWidth = container.offsetWidth / elements.length;
    function showPage(page) {
        const offset = page * elementWidth;
        container.style.transform = `translateX(-${offset}px)`;
        container.style.transition = 'transform 0.5s ease-in-out';
        if (elements.length <= 5) {

            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'none';
        }
        else if (page >= end_page || end_page <= 0) {
            NextBtn.style.display = 'none'
            PrevBtn.style.display = 'block';
        }
        else if (page == 0) {
            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'block';
        }
        else {
            NextBtn.style.display = 'block';
            PrevBtn.style.display = 'block';
        }
    }

    document.querySelector("#rfuNext").addEventListener('click', () => {
        if (current_page <= end_page) {
            current_page++;
            showPage(current_page);
        }
        if (current_page > 1) {
            PrevBtn.style.display = 'block';

        }
        console.log("Next: " + current_page);
    });

    document.querySelector("#rfuPrev").addEventListener('click', () => {
        if (current_page > 0) {
            current_page--;
            showPage(current_page);
        }
        console.log("Prev: " + current_page);

    });

    showPage(current_page);
}

// Culinary Corner

function Culinary() {
    const container = document.querySelector('#ccelements');
    const elements = document.querySelectorAll('#ccelement');
    const NextBtn = document.querySelector("#ccNext");
    const PrevBtn = document.querySelector("#ccPrev");
    let current_page = 0;
    let end_page = elements.length - 5;
    const elementWidth = container.offsetWidth / elements.length;
    function showPage(page) {
        const offset = page * elementWidth;
        container.style.transform = `translateX(-${offset}px)`;
        container.style.transition = 'transform 0.5s ease-in-out';
        if (elements.length <= 5) {

            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'none';
        }
        else if (page >= end_page || end_page <= 0) {
            NextBtn.style.display = 'none'
            PrevBtn.style.display = 'block';
        }
        else if (elements.length < 5) {
            NextBtn.style.display = 'none';
            PrevBtn.style.display = 'none';
        }
        else if (page == 0) {
            PrevBtn.style.display = 'none';
            NextBtn.style.display = 'block';
        }
        else {
            NextBtn.style.display = 'block';
            PrevBtn.style.display = 'block';
        }
    }

    document.querySelector("#ccNext").addEventListener('click', () => {
        if (current_page <= end_page) {
            current_page++;
            showPage(current_page);
        }
        if (current_page > 1) {
            PrevBtn.style.display = 'block';

        }
        console.log("Next: " + current_page);
    });

    document.querySelector("#ccPrev").addEventListener('click', () => {
        if (current_page > 0) {
            current_page--;
            showPage(current_page);
        }
        console.log("Prev: " + current_page);

    });

    showPage(current_page);
}
