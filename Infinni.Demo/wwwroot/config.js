window.InfinniUI = window.InfinniUI || {};
window.InfinniUI.config = window.InfinniUI.config || {};

// Перекрываем дефолтные конфиги, лежащие в InfinniUI/app/config.js

window.InfinniUI.config.cacheMetadata = false;

// Для разработки
window.InfinniUI.config.serverUrl = 'http://localhost:5000';

// Тестовый сервер
// window.InfinniUI.config.serverUrl = 'http://localhost:9900';

window.InfinniUI.config.configId = 'SchoolStore';
window.InfinniUI.config.configName = 'School Store';

window.InfinniUI.config.homePage = {Path: 'StartPage'};

window.SchoolStore = window.SchoolStore || {};

window.SchoolStore.scripts = window.SchoolStore.scripts || {};
window.SchoolStore.common = window.SchoolStore.common || {};
window.SchoolStore.router = window.SchoolStore.router || {};
window.SchoolStore.services = window.SchoolStore.services || {};
window.SchoolStore.extensions = window.SchoolStore.extensions || {};

window.SchoolStore.VERSION = '1.1.1.1491882037';
window.SchoolStore.HASH = 'development';
