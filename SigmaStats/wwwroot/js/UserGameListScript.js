async function loadUserGames(steamId) {

    const gamesBody = document.getElementById('ownedGamesBody');
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
        </tr>
    `).join('');
    }

    try {
        const response = await fetch(`https://localhost:7230/api/steam/owned-games?steamId=${steamId}`);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const result = await response.json();
        if (!result.success) {
            throw new Error(result.errorMessage || 'Failed to load user games');
        }
        const games = result.games.$values || result.games;
        if (Array.isArray(games)) {
            renderUserGames(games);

            const totalPlaytime = games.reduce((total, game) => total + game.playtime, 0);
            document.getElementById('totalPlaytime').textContent = `${Math.round(totalPlaytime / 60)} hours`;
        } else {
            throw new Error('The games data is not an array.');
        }

    } catch (error) {
        console.error('Error loading user games:', error);
        const gamesBody = document.getElementById('ownedGamesBody');
        if (gamesBody) {
            gamesBody.innerHTML = `
                <tr>
                    <td colspan="3" class="text-danger">${error.message}</td>
                </tr>
            `;
        }
    }
}



function renderUserGames(games) {
    const tbody = document.getElementById('ownedGamesBody');
    if (!tbody) return;

    const gameArray = Array.isArray(games) ? games : [];
    tbody.innerHTML = gameArray.map((game, index) => `
        <tr class="game-card">
            <td class="align-middle">
                <div class="rank-badge">${index + 1}</div>
            </td>
            <td class="align-middle">
                <div class="d-flex align-items-center">
                    <div class="game-image-container" style="min-width: 184px; height: 69px;">
                        <img src="${game.capsuleImage || ''}"
                             alt="${game.name || 'Game'}"
                             loading="eager"
                             fetchpriority="high"
                             decoding="async"
                             width="184"
                             height="69"
                             class="game-image me-3">
                    </div>
                    <h6 class="mb-0">
                        <a href="/games/details/${game.appId}" class="text-white text-decoration-none">
                            ${game.name || 'Unknown Game'}
                        </a>
                    </h6>
                </div>
            </td>
            <td class="align-middle text-highlight">
                ${Math.round(game.playtime / 60)} hours
            </td>
        </tr>
    `).join('');
}


document.addEventListener('DOMContentLoaded', () => {
    const loadBtn = document.getElementById('loadGamesButton');
    const steamIdInput = document.getElementById('steamIdInput');

    if (loadBtn && steamIdInput) {
        loadBtn.addEventListener('click', () => {
            const steamId = steamIdInput.value.trim();
            if (steamId) {
                loadUserGames(steamId);
            } else {
                alert('Please enter a valid SteamID.');
            }
        });

        steamIdInput.addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                loadBtn.click();
            }
        });
    }
});
