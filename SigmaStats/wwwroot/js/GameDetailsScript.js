document.addEventListener("DOMContentLoaded", async () => {
    const container = document.getElementById("gameDetailsContainer");
    const appId = window.location.pathname.split("/").pop();

    try {
        const response = await fetch(`https://localhost:7230/api/steam/game-details/${appId}`);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const contentType = response.headers.get("Content-Type");
        const text = await response.text();

        if (!text || !contentType || !contentType.includes("application/json")) {
            console.error("Non-JSON or empty response:", text);
            throw new Error("Invalid or empty response from server.");
        }

        const game = JSON.parse(text);
        renderGameDetails(game, container);

    } catch (error) {
        container.innerHTML = `<p class="text-danger">${error.message}</p>`;
        console.error("Error loading game details:", error);
    }
});

function renderGameDetails(game, container) {
    container.innerHTML = `
        <div class="game-details-container">
            <div class="game-details-card">
                <!-- Game Header -->
                <div class="game-header">
                    ${game.headerImage ? `
                        <img src="${game.headerImage}" class="game-header-image" alt="${game.name}" />
                    ` : ''}
                    <div class="game-header-content">
                        <h1 class="game-title">${game.name || "No Name Available"}</h1>
                        <div class="game-genres">
                            ${game.genres ? game.genres.map(genre => `
                                <span class="game-genre-badge">${genre}</span>
                            `).join('') : ''}
                        </div>
                    </div>
                </div>

                <div class="game-details-body">
                    <!-- Short Description -->
                    <div class="game-short-desc">
                        <p>"${game.shortDescription || "No short description available."}"</p>
                    </div>

                    <!-- Detailed Description -->
                    <div class="game-section fade-in-section">
                        <h4 class="section-title"><i class="bi bi-info-circle-fill"></i>Description</h4>
                        <p class="section-content">${game.detailedDescription || "No detailed description available."}</p>
                    </div>

                    <!-- System Requirements -->
                    <div class="game-section fade-in-section">
                        <h4 class="section-title"><i class="bi bi-pc-display-horizontal"></i>System Requirements</h4>
                        <div class="requirements-grid">
                            ${game.pcRequirements?.Minimum ? `
                                <div class="requirement-card">
                                    <h5><i class="bi bi-windows"></i> Windows</h5>
                                    <div class="requirement-text">${game.pcRequirements.Minimum}</div>
                                </div>` : ''}
                            ${game.pcRequirements?.Minimum ? `
                                <div class="requirement-card">
                                    <h5><i class="bi bi-apple"></i> Mac</h5>
                                    <div class="requirement-text">${game.pcRequirements.Minimum}</div>
                                </div>` : ''}
                            ${game.pcRequirements?.Minimum ? `
                                <div class="requirement-card">
                                    <h5><i class="bi bi-ubuntu"></i> Linux</h5>
                                    <div class="requirement-text">${game.pcRequirements.Minimum}</div>
                                </div>` : ''}
                        </div>
                    </div>

                    <!-- Metadata Section -->
                    <div class="metadata-section fade-in-section">
                        <div class="metadata-card">
                            <h4 class="section-title"><i class="bi bi-person-workspace"></i>Developers & Publishers</h4>
                            <div class="metadata-content">
                                <div class="metadata-item">
                                    <span class="metadata-label">Developer</span>
                                    <p>${(game.developers || []).join(", ") || "N/A"}</p>
                                </div>
                                <div class="metadata-item">
                                    <span class="metadata-label">Publisher</span>
                                    <p>${(game.publishers || []).join(", ") || "N/A"}</p>
                                </div>
                            </div>
                        </div>
                        <div class="metadata-card">
                            <h4 class="section-title"><i class="bi bi-tags"></i>Genres & Categories</h4>
                            <div class="metadata-content">
                                <div class="metadata-item">
                                    <span class="metadata-label">Genres</span>
                                    <div class="badges-container">
                                        ${(game.genres || []).map(genre => `
                                            <span class="content-badge">${genre}</span>
                                        `).join('') || "N/A"}
                                    </div>
                                </div>
                                <div class="metadata-item">
                                    <span class="metadata-label">Categories</span>
                                    <div class="badges-container">
                                        ${(game.categories || []).map(cat => `
                                            <span class="content-badge">${cat}</span>
                                        `).join('') || "N/A"}
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Languages -->
                    <div class="game-section fade-in-section">
                        <h4 class="section-title"><i class="bi bi-translate"></i>Languages Supported</h4>
                        <div class="badges-container">
                            ${(game.supportedLanguages || []).map(lang => `
                                <span class="language-badge">${lang}</span>
                            `).join('') || "N/A"}
                        </div>
                    </div>

                    <!-- Screenshots -->
                    ${Array.isArray(game.screenshots) && game.screenshots.length > 0 ? `
                        <div class="game-section fade-in-section">
                            <h4 class="section-title"><i class="bi bi-images"></i>Screenshots</h4>
                            <div class="screenshots-grid">
                                ${game.screenshots.map(src => `
                                    <div class="screenshot-container">
                                        <img src="${src}" class="screenshot-image" alt="Screenshot" />
                                        <div class="screenshot-overlay">
                                            <i class="bi bi-zoom-in"></i>
                                        </div>
                                    </div>
                                `).join('')}
                            </div>
                        </div>
                    ` : ''}

                    ${Array.isArray(game.videos) && game.videos.length > 0 ? `
                        <div class="game-section fade-in-section">
                            <h4 class="section-title"><i class="bi bi-film"></i>Trailers</h4>
                            <div class="trailers-grid">
                                ${game.videos.map(url => `
                                    <video class="trailer-video" controls preload="none" poster="${game.headerImage}">
                                        <source src="${url}" type="video/webm">
                                        Your browser does not support the video tag.
                                    </video>
                                `).join('')}
                            </div>
                        </div>
                    ` : ''}


                    <!-- Website Link -->
                    ${game.website ? `
                        <div class="game-section fade-in-section">
                            <h4 class="section-title"><i class="bi bi-link-45deg"></i>Links</h4>
                            <a href="${game.website}" target="_blank" class="website-button">
                                <i class="bi bi-box-arrow-up-right"></i> Official Website
                            </a>
                        </div>
                    ` : ''}
                </div>
            </div>
        </div>
    `;

    initGameDetailsHoverEffects(container);
}

function initGameDetailsHoverEffects(container) {
    container.querySelectorAll('.screenshot-container').forEach(el => {
        el.addEventListener('mouseenter', () => {
            el.querySelector('.screenshot-image').classList.add('screenshot-hover');
            el.querySelector('.screenshot-overlay').classList.add('overlay-visible');
        });
        el.addEventListener('mouseleave', () => {
            el.querySelector('.screenshot-image').classList.remove('screenshot-hover');
            el.querySelector('.screenshot-overlay').classList.remove('overlay-visible');
        });
    });

    container.querySelectorAll('.requirement-card, .metadata-card').forEach(el => {
        el.addEventListener('mouseenter', () => {
            el.classList.add('card-hover');
        });
        el.addEventListener('mouseleave', () => {
            el.classList.remove('card-hover');
        });
    });

    const websiteBtn = container.querySelector('.website-button');
    if (websiteBtn) {
        websiteBtn.addEventListener('mouseenter', () => {
            websiteBtn.classList.add('button-hover');
        });
        websiteBtn.addEventListener('mouseleave', () => {
            websiteBtn.classList.remove('button-hover');
        });
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate__animated', 'animate__fadeIn');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    container.querySelectorAll('.fade-in-section').forEach(el => {
        observer.observe(el);
    });
}