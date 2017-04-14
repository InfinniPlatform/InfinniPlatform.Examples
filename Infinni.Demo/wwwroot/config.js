window.InfinniUI = window.InfinniUI || {};
window.InfinniUI.config = window.InfinniUI.config || {};

// Перекрываем дефолтные конфиги, лежащие в InfinniUI/app/config.js

window.InfinniUI.config.cacheMetadata = false;

window.InfinniUI.config.serverRootUrl = 'http://testcluster.infinnity.local';
window.InfinniUI.config.serverUrl = window.InfinniUI.config.serverRootUrl + '/api';
window.InfinniUI.config.configId = 'Infinni.Demo';
window.InfinniUI.config.configName = 'Infinni.Demo';

window.InfinniUI.config.homePage = {Path: 'StartPage'};

window.SchoolStore = window.SchoolStore || {};

window.SchoolStore.scripts = window.SchoolStore.scripts || {};
window.SchoolStore.common = window.SchoolStore.common || {};
window.SchoolStore.router = window.SchoolStore.router || {};
window.SchoolStore.services = window.SchoolStore.services || {};
window.SchoolStore.extensions = window.SchoolStore.extensions || {};

window.SchoolStore.VERSION = '1.0.0.0';
window.SchoolStore.HASH = 'development';
