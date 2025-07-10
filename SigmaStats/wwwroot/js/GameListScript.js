async function loadTopGames() {
    const gamesBody = document.getElementById('gamesBody');
    if (gamesBody) {
        gamesBody.innerHTML = Array.from({ length: 10 }).map(() => `
            <tr class="game-card shimmer">
                <td><div class="skeleton skeleton-rank"></div></td>
                <td>
                    <div class="d-flex align-items-center">
                        <div class="skeleton skeleton-image me-3"></div>
                        <div class="skeleton skeleton-text" style="width: 200px;"></div>
                    </div>
                </td>
                <td><div class="skeleton skeleton-text" style="width: 80px;"></div></td>
                <td><div class="skeleton skeleton-text" style="width: 80px;"></div></td>
            </tr>
        `).join('');
    }

    try {
        const response = await fetch('https://localhost:7230/api/steam/top-games');

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const result = await response.json();

        if (!result.success) {
            throw new Error(result.errorMessage || 'Failed to load games');
        }

        renderGames(result.games);
    } catch (error) {
        console.error('Error loading games:', error);
        if (gamesBody) {
            gamesBody.innerHTML = `
                <tr>
                    <td colspan="4" class="text-danger">${error.message}</td>
                </tr>
            `;
        }
    }
}

function renderGames(games) {
    const tbody = document.getElementById('gamesBody');
    if (!tbody) return;
    const gameArray = games.$values || games;
    if (Array.isArray(gameArray)) {
        tbody.innerHTML = gameArray.map((game, index) => `
            <tr class="game-card">
                <td class="align-middle">
                    <div class="rank-badge">${game.rank}</div>
                </td>
                <td class="align-middle">
                    <div class="d-flex align-items-center">
                        <div class="game-image-container" style="min-width: 184px; height: 69px;">
                            <img src="${game.capsuleImage}"
                                 alt="${game.name}"
                                 loading="eager"
                                    fetchpriority="high"
                                 decoding="async"
                                 width="184"
                                 height="69" 
                                 class="game-image me-3">
                        </div>
                        <h6 class="mb-0">
                            <a href="/games/details/${game.appId}" 
                            class="text-white text-decoration-none">${game.name}</a>
                        </h6>

                    </div>
                </td>
                <td class="align-middle player-count">${game.currentPlayers.toLocaleString()}</td>
                <td class="align-middle peak-count">${game.peakInGame.toLocaleString()}</td>
            </tr>
        `).join('');
    } else {
        console.error("Expected an array, but received:", games);
    }
}

document.addEventListener('DOMContentLoaded', loadTopGames);