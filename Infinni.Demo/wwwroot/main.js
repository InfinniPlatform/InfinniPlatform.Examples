(function($) {
  var $target = $('#page-content');
  var host = InfinniUI.config.serverUrl;

  InfinniUI.global.messageBus.subscribe('onViewCreated', function(context, args) {
    InfinniUI.RouterService.setContext(args.value.view.getContext());

    if (args.value.openMode === 'Default') {
      window.contextApp = args.value.view;
    }
  });

  InfinniUI.providerRegister.register('MetadataDataSource', function(metadataValue) {
    return new InfinniUI.Providers.MetadataProviderREST(new QueryConstructorMetadata(host, metadataValue));
  });

  overrideExecuteScript();
  overrideScriptByNameOrBody();
  registerExtensionPanels();

  InfinniUI.openHomePage($target);

  /**
   * Конструктор запросов метаданных
   * @param {any} host - адрес сервера приложения
   * @param {any} metadata - запрашиваемые метаданные
   */
  function QueryConstructorMetadata(host, metadata) {
    this.constructMetadataRequest = function() {
      return {
        requestUrl: window.InfinniUI.config.serverRootUrl + '/Views/' + metadata.Path + '.json?v1.1.1',
        method: 'GET'
      };
    };
  }

  /**
   * Переопределение функции InfinniUI.ScriptExecutor.executeScript
   */
  function overrideExecuteScript() {
    var _executeScript = InfinniUI.ScriptExecutor.prototype.executeScript;

    InfinniUI.ScriptExecutor.prototype.executeScript = function(scriptName, args) {
      return _executeScript.call(this, modifyScriptName.call(this, scriptName), args);
    };
  }

  /**
   * Переопредление функции InfinniUI.DataBindingBuilder.scriptByNameOrBody
   */
  function overrideScriptByNameOrBody() {
    var _scriptByNameOrBody = InfinniUI.DataBindingBuilder.prototype.scriptByNameOrBody;

    InfinniUI.DataBindingBuilder.prototype.scriptByNameOrBody = function(nameOrBody, context) {
      return _scriptByNameOrBody.call(this, modifyScriptName.call(this, nameOrBody, context), context);
    };
  }

  /**
   * Модификация прикладного скрипта, если он содержит префикс "SS:"
   * @param {String} scriptName - Скрипт
   * @param {Object} [context] - Контекст представления
   * @return {String} - Скрипт из секции SchoolStore.scripts
   */
  function modifyScriptName(scriptName, context) {
    if (scriptName.substring(0, 3) === 'SS:') {
      context = context || this.parent.getContext();

      var viewName = context.view.getName();
      var funcName = viewName.substring(0, 1).toLowerCase() + viewName.substring(1) + scriptName.substring(3);

      scriptName = '{return SchoolStore.scripts.' + funcName + '(context, args);}';
    }

    return scriptName;
  }

  /**
   * Регистрация всех extension панелей, так как они все должны быть в window
   */
  function registerExtensionPanels() {
    _.each(SchoolStore.extensions, function(value, key) {
      window[key] = value;
    });
  }
})(window.jQuery);
