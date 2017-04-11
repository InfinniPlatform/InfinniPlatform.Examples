(function(router) {
  InfinniUI.config.Routes = [];

  mapRoute('DefaultRoute', '/');
  mapRoute('SignInRoute', '/login');
  mapRoute('CatalogGoodsRoute', '/catalog/services');
  mapRoute('ServiceProvidersRoute', '/providers');
  mapRoute('ServiceProviderCreateRoute', '/provider/create');
  mapRoute('ServiceProviderEditRoute', '/provider/edit/<%Id%>');
  mapRoute('ServiceProviderPreviewRoute', '/provider/preview/<%Id%>');

  function mapRoute(name, path) {
    InfinniUI.config.Routes.push({
      Name: name,
      Path: path,
      Action: '{SchoolStore.router.onRouteChanged(context, args);}'
    });
  }

  router.goToUrl = function(context, options, callback) {
    if (_.isString(options)) {
      options = {path: options};
    }

    if (_.isFunction(callback)) {
      _.delay(callback, 50);
    }

    var path = options.path;
    var replace = options.replace;

    var route = _.find(InfinniUI.config.Routes, function(r) {
      return r.Path === path;
    });

    context.global.executeAction(context, {
      RouteToAction: {
        Name: route.Name,
        Replace: replace
      }
    });
  };

  router.onRouteChanged = function(context, args) {
    if (args.routeParams.user) {
      if (window.contextApp.getName() === 'HomePage') {
        handleAuthorizedRoute(context, args);
      } else {
        redirectToHomePage(context, function() {
          handleAuthorizedRoute(context, args);
        });
      }
    } else {
      handleUnauthorizedRoute(context, args);
    }
  };

  function redirectToHomePage(context, callback) {
    context.global.executeAction(context, {
      OpenAction: {
        LinkView: {
          AutoView: {
            Path: 'Common/HomePage'
          }
        }
      }
    }, callback);
  }

  function handleAuthorizedRoute(context, args) {
    if (args.name === 'DefaultRoute') {
      router.goToUrl(context, {
        path: '/catalog/services',
        replace: true
      });
    } else {
      switch (args.name) {
        case 'CatalogGoodsRoute':
          handleCatalogGoodsRoute(context, args.params);
          break;
        case 'ServiceProvidersRoute':
          handleServiceProvidersRoute(context, args.params);
          break;
        case 'ServiceProviderCreateRoute':
          handleServiceProviderCreateRoute(context, args.params);
          break;
        case 'ServiceProviderEditRoute':
          handleServiceProviderEditRoute(context, args.params);
          break;
        case 'ServiceProviderPreviewRoute':
          handleServiceProviderPreviewRoute(context, args.params);
          break;
        default:
          return;
      }
    }
  }

  function handleUnauthorizedRoute(context, args) {
    if (args.name !== 'SignInRoute') {
      router.goToUrl(context, {
        path: '/login',
        replace: true
      });
    } else {
      context.global.executeAction(context, {
        OpenAction: {
          LinkView: {
            AutoView: {
              Path: 'Common/SignInView'
            }
          }
        }
      });
    }
  }

  function handleCatalogGoodsRoute(context, params) {
    context.global.executeAction(context, {
      OpenAction: {
        LinkView: {
          AutoView: {
            Path: 'CatalogGoods/CatalogGoodsView',
            OpenMode: 'Container',
            Container: 'Content'
          }
        }
      }
    });
  }

  function handleServiceProvidersRoute(context, params) {
    context.global.executeAction(context, {
      OpenAction: {
        LinkView: {
          AutoView: {
            Path: 'ServiceProvider/ServiceProviderView',
            OpenMode: 'Container',
            Container: 'Content'
          }
        }
      }
    });
  }

  function handleServiceProviderCreateRoute(context, params) {
    context.global.executeAction(context, {
      AddAction: {
        SourceValue: {
          Source: 'MainDataSource'
        },
        LinkView: {
          AutoView: {
            Path: 'ServiceProvider/ServiceProviderEditView',
            OpenMode: 'Container',
            Container: 'Content'
          }
        }
      }
    });
  }

  function handleServiceProviderEditRoute(context, params) {
    context.global.executeAction(context, {
      OpenAction: {
        LinkView: {
          AutoView: {
            Path: 'ServiceProvider/ServiceProviderEditView',
            OpenMode: 'Container',
            Container: 'Content',
            Parameters: [
              {
                Name: 'DocumentId',
                Value: params[0]
              }
            ]
          }
        }
      }
    });
  }

  function handleServiceProviderPreviewRoute(context, params) {
    context.global.executeAction(context, {
      OpenAction: {
        LinkView: {
          AutoView: {
            Path: 'ServiceProvider/ServiceProviderPreview',
            OpenMode: 'Container',
            Container: 'Content',
            Parameters: [
              {
                Name: 'DocumentId',
                Value: params[0]
              }
            ]
          }
        }
      }
    });
  }
})(SchoolStore.router);
