;(function() {
//###common.js
console.groupCollapsed('Config version');
console.log('SchoolStore  %s (%s)', SchoolStore.VERSION, SchoolStore.HASH);
console.log('InfinniUI    %s', InfinniUI.VERSION);
console.groupEnd();

/**
 * Проверка значения на заполненность
 * @param {any} value - Входное значение
 * @return {Boolean} - Результат
 */
SchoolStore.common.isNullOrWhiteSpace = function(value) {
  return !Boolean(value) || /^\s+$/.test(value);
};

/**
 * Получить список ошибок
 * @param {Array<Object>} fields - Список полей (field: поле, message: кастомное сообщение, caption: наименование поля)
 * @return {Object} - Результат проверки
 */
SchoolStore.common.getValidationErrors = function(fields) {
  var result = {Items: []};

  _.each(fields, function(field) {
    if (SchoolStore.common.isNullOrWhiteSpace(field.field)) {
      result.Items.push({Message: field.message || 'Поле "' + field.caption + '" обязательно для заполнения.'});
    }
  });

  result.IsValid = result.Items.length === 0;

  return result;
};

SchoolStore.common.serviceCaptionLabelTextConverter = function(context, args) {
  if (args.binding.getElement().getTag() === undefined) {
    return '';
  }

  var property = args.binding.attributes.sourceProperty;
  var index = _.last(property.split('.'));
  if (args.binding.getElement().getTag() === 'ServiceProviderPreview') {
    return (parseInt(index, 10) + 1);
  } else {
    return 'Услуга ' + (parseInt(index, 10) + 1);
  }
};

//###StartPage\OnLoaded.js
SchoolStore.scripts.startPageOnLoaded = function(context, args) {
  InfinniUI.user.onReady(function(user) {
    SchoolStore.currentUser = user;
    InfinniUI.RouterService.setParams({user: user});
    InfinniUI.RouterService.startRouter();
  });
};

//###CatalogGoods\CatalogGoodsView\DeleteButtonOnClick.js
SchoolStore.scripts.catalogGoodsCatalogGoodsViewDeleteButtonOnClick = function(context, args) {
  var selectedItem = context.dataSources.CatalogGoodsDataSource.getSelectedItem();
  var serviceProviders = context.dataSources.ServiceProviderDataSource.getItems();

  if (selectedItem.ParentId === 'ROOT_CATEGORY') {
    var supplierWithCategory = _.find(serviceProviders, function(item) {
      return item.CategoryId === selectedItem._id;
    });

    if (!supplierWithCategory) {
      new InfinniUI.MessageBox({
        type: 'error',
        text: 'Подтвердите удаление категории',
        buttons: [
          {name: 'Ок', onClick: deleteCategoryOrSubCategory},
          {name: 'Отмена'}
        ]
      });
    } else {
      toastr.error('Категория, содержащая поставщиков, не может быть удалена');
    }
  } else {
    var supplierWithSubcategory = _.find(serviceProviders, function(item) {
      return _.find(item.SubCategories, function(subCategory) {
        return subCategory.CategoryId === selectedItem._id;
      });
    });

    if (!supplierWithSubcategory) {
      new InfinniUI.MessageBox({
        type: 'error',
        text: 'Подтвердите удаление подкатегории',
        buttons: [
          {name: 'Ок', onClick: deleteCategoryOrSubCategory},
          {name: 'Отмена'}
        ]
      });
    } else {
      toastr.error('Подкатегория, содержащая поставщиков, не может быть удалена');
    }
  }

  function deleteCategoryOrSubCategory() {
    context.global.executeAction(context, {
      DeleteAction: {
        Accept: false,
        DestinationValue: {
          Source: 'CatalogGoodsDataSource',
          Property: '$'
        }
      }
    });
  }
};

//###CatalogGoods\CategoryGoodsEditView\ConverterLabelText.js
SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewConverterLabelText = function(context, args) {
  if (args.value === 'add') {
    if (!context.dataSources.CatalogGoodsDataSource.getSelectedItem().ParentId) {
      return 'Добавить категорию';
    } else {
      return 'Добавить подкатегорию';
    }
  } else if (context.dataSources.CatalogGoodsDataSource.getSelectedItem().ParentId === 'ROOT_CATEGORY') {
    return 'Редактировать категорию';
  } else {
    return 'Редактировать подкатегорию';
  }
};

//###CatalogGoods\CategoryGoodsEditView\OnLoaded.js
SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewOnLoaded = function(context, args) {
  if (context.parameters.Type.getValue() === 'add') {
    context.dataSources.CatalogGoodsDataSource.setProperty('$.ParentId', context.parameters.CategoryType.getValue()._id);
  }
};

//###CatalogGoods\CategoryGoodsEditView\SaveButtonOnClick.js
SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewSaveButtonOnClick = function(context, args) {
  var directoryCategories = context.dataSources.CatalogGoodsDataSource.getItems();
  var filledDirectoryCategories = _.filter(directoryCategories, function(category) {
    return category.Name;
  });

  if (filledDirectoryCategories.length === 0) {
    toastr.error('Поле "Название" не может быть пустым');
    return;
  }

  context.dataSources.AddCategoryDataSource.createItem();
  context.dataSources.AddCategoryDataSource.setProperty('$.DirectoryCategories', filledDirectoryCategories);

  context.global.executeAction(context, {
    SaveAction: {
      DestinationValue: {
        Source: 'AddCategoryDataSource'
      }
    }
  });
};

//###Common\HomePage\SignOutButtonOnClick.js
SchoolStore.scripts.homePageSignOutButtonOnClick = function(context, args) {
  InfinniUI.global.session.signOut(function() {
    window.location.reload();
  });
};

//###Common\SignInView\MainDataSourceOnPropertyChanged.js
SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged = function(context, args) {
  var user = args.source.getSelectedItem();
  var button = context.controls.SignInButton;

  if (!button) {
    return;
  }

  if (user && user.username && user.password) {
    button.setEnabled(true);
  } else {
    button.setEnabled(false);
  }
};

//###Common\SignInView\SignInButtonOnClick.js
SchoolStore.scripts.signInViewSignInButtonOnClick = function(context, args) {
  var user = context.dataSources.MainDataSource.getSelectedItem();

  InfinniUI.global.session.signInInternal(user.username, user.password, user.passwordRemember, function(user) {
    SchoolStore.currentUser = user;
    InfinniUI.RouterService.setParams({user: user});
    SchoolStore.router.goToUrl(context, {
      path: '/',
      replace: true
    });
  }, function() {
    toastr.error('Неверный логин или пароль');
  });
};

//###ServiceProvider\ServiceProviderView\CityComboBoxOnValueChanged.js
SchoolStore.scripts.serviceProviderViewCityComboBoxOnValueChanged = function(context, args) {
  var filter = args.newValue ? InfinniUI.StringUtils.format('eq(CityId,\'{0}\')', [args.newValue._id]) : '';
  context.dataSources.ServiceProviderDataSource.setFilter(filter);
};

//###ServiceProvider\ServiceProviderView\ImageBoxOnClick.js
SchoolStore.scripts.serviceProviderViewImageBoxOnClick = function(context, args) {
  var value = args.source.getTag();
  context.dataSources.ServiceProviderDataSource.setSelectedItem(value);
  context.global.executeAction(context, {
    RouteToAction: {
      Name: 'ServiceProviderPreviewRoute',
      Params: [
        {
          Name: 'Id',
          Value: {
            Source: 'ServiceProviderDataSource',
            Property: '$._id'
          }
        }
      ]
    }
  });
};

//###ServiceProvider\ServiceProviderView\SortSuppliers.js
SchoolStore.scripts.serviceProviderViewSortSuppliersOnClick = function(context, args) {
  var tag = args.source.getTag();
  if (tag === 'DateOfCreation') {
    context.dataSources.ServiceProviderDataSource.setOrder('desc(_header._created)');
    context.controls.DateOfCreationButton.setBackground('primary1');
    context.controls.AlphabetButton.setBackground('');
  } else {
    context.dataSources.ServiceProviderDataSource.setOrder('asc(Name)');
    context.controls.AlphabetButton.setBackground('primary1');
    context.controls.DateOfCreationButton.setBackground('');
  }
};

//###ServiceProvider\ServiceProviderEditView\AddServiceButtonOnClick.js
SchoolStore.scripts.serviceProviderEditViewAddServiceButtonOnClick = function(context, args) {
  var mds = context.dataSources.MainDataSource;
  var services = mds.getProperty('$.Services');

  services.push({});
  mds.setProperty('$.Services', _.clone(services));
};

//###ServiceProvider\ServiceProviderEditView\HeaderLabelTextConverter.js
SchoolStore.scripts.serviceProviderEditViewHeaderLabelTextConverter = function(context, args) {
  if (args.value && args.value._header) {
    return 'Редактирование поставщика';
  } else {
    return 'Создание поставщика';
  }
};

//###ServiceProvider\ServiceProviderEditView\MainDataSourceOnSuccess.js
SchoolStore.scripts.serviceProviderEditViewMainDataSourceOnSuccess = function(context, args) {
  var item = args.item;

  if (item._header) {
    context.global.executeAction(context, {
      RouteToAction: {
        Name: 'ServiceProviderPreviewRoute',
        Params: [
          {
            Name: 'Id',
            Value: {
              Source: 'MainDataSource',
              Property: '$._id'
            }
          }
        ]
      }
    });
    toastr.success('Поставщик успешно изменён');
  } else {
    context.global.executeAction(context, {
      RouteToAction: {
        Name: 'ServiceProviderPreviewRoute',
        Params: [
          {
            Name: 'Id',
            Value: {
              Source: 'MainDataSource',
              Property: '$._id'
            }
          }
        ]
      }
    });
    toastr.success('Поставщик успешно создан');
  }
};

//###ServiceProvider\ServiceProviderEditView\MainDataSourceValidationErrors.js
SchoolStore.scripts.serviceProviderEditViewMainDataSourceValidationErrors = function(context, item) {
  var result = SchoolStore.common.getValidationErrors([
    {field: item.Name, caption: 'Название'},
    {field: item.Address, caption: 'Адрес'},
    {field: item.CityId, caption: 'Город'},
    {field: item.Description, caption: 'Описание'},
    {field: item.Email, caption: 'E-mail'},
    {field: item.Phone, caption: 'Телефон'},
    {field: item.CategoryId, caption: 'Категория'}
  ]);

  var services = context.dataSources.MainDataSource.getProperty('$.Services');
  var isValid = _.all(services, function(service) {
    return !SchoolStore.common.isNullOrWhiteSpace(service.Name) &&
      !SchoolStore.common.isNullOrWhiteSpace(service.Description);
  });

  if (!isValid) {
    result.IsValid = false;
    result.Items.push({Message: 'Заполнены не все услуги.'});
  }

  return result;
};

//###ServiceProvider\ServiceProviderEditView\OnLoaded.js
SchoolStore.scripts.serviceProviderEditViewOnLoaded = function(context, args) {
  var provider = context.dataSources.MainDataSource.getSelectedItem();

  if (isEdit(provider)) {
    context.controls.CityComboBox.setValue(provider.City);
    context.controls.CategoryComboBox.setValue(provider.Category);
  } else {
    context.dataSources.MainDataSource.setProperty('$.Services', [{}]);
  }

  function isEdit(provider) {
    return typeof provider._header !== 'undefined';
  }
};

//###ServiceProvider\ServiceProviderEditView\RemoveServiceButtonOnClick.js
SchoolStore.scripts.serviceProviderEditViewRemoveServiceButtonOnClick = function(context, args) {
  var service = args.source.getTag();
  var services = context.dataSources.MainDataSource.getProperty('$.Services');

  if (_.indexOf(services, service) < 1) {
    return;
  }

  services = _.reject(services, function(s) {
    return s === service;
  });

  context.dataSources.MainDataSource.setProperty('$.Services', services);
};

//###ServiceProvider\ServiceProviderEditView\RemoveServiceButtonVisibleConverter.js
SchoolStore.scripts.serviceProviderEditViewRemoveServiceButtonVisibleConverter = function(context, args) {
  var property = args.binding.attributes.sourceProperty;
  var index = _.last(property.split('.'));

  return index !== '0';
};

//###ServiceProvider\ServiceProviderEditView\ServiceCaptionLabelTextConverter.js
SchoolStore.scripts.serviceProviderEditViewServiceCaptionLabelTextConverter = function(context, args) {
  var property = args.binding.attributes.sourceProperty;
  var index = _.last(property.split('.'));

  return 'Услуга ' + (parseInt(index, 10) + 1);
};

//###ServiceProvider\ServiceProviderEditView\SubCategoriesDataSourceOnItemsUpdated.js
SchoolStore.scripts.serviceProviderEditViewSubCategoriesDataSourceOnItemsUpdated = function(context, args) {
  var mainDs = context.dataSources.MainDataSource;
  var categoryId = mainDs.getProperty('$.CategoryId');
  var subCategories = mainDs.getProperty('$.SubCategories') || [];

  if (!categoryId) {
    return;
  }

  var isSubCategoriesBelongToSelectedCategory = _.any(subCategories, function(subcategory) {
    return subcategory.Category && subcategory.Category.ParentId === categoryId;
  });

  if (isSubCategoriesBelongToSelectedCategory) {
    var checkBoxes = context.controls.SubCategoriesStackPanel.findAllChildrenByType('CheckBox');
    var subCategoriesIds = _.map(subCategories, function(subCategory) {
      return subCategory.CategoryId;
    });

    _.each(checkBoxes, function(checkBox) {
      if (_.indexOf(subCategoriesIds, checkBox.getTag()._id) !== -1) {
        checkBox.setValue(true);
      }
    });
  } else {
    mainDs.setProperty('$.SubCategories', []);
  }
};

//###ServiceProvider\ServiceProviderEditView\SubCategoryCheckBoxOnValueChanged.js
SchoolStore.scripts.serviceProviderEditViewSubCategoryCheckBoxOnValueChanged = function(context, args) {
  var mainDs = context.dataSources.MainDataSource;
  var subCategories = mainDs.getProperty('$.SubCategories') || [];
  var subCategory = args.source.getTag();

  var exist = _.any(subCategories, function(s) {
    return s.CategoryId === subCategory._id;
  });

  if (exist === args.newValue) {
    return;
  }

  if (args.newValue) {
    subCategories.push({CategoryId: subCategory._id});
  } else {
    subCategories = _.reject(subCategories, function(s) {
      return s.CategoryId === subCategory._id;
    });
  }

  mainDs.setProperty('$.SubCategories', _.clone(subCategories));
};

//###ServiceProvider\ServiceProviderPreview\DeleteButtonOnClick.js
SchoolStore.scripts.serviceProviderPreviewDeleteButtonOnClick = function(context, args) {
  new InfinniUI.MessageBox({
    type: 'error',
    text: 'Подтвердите удаление поставщика',
    buttons: [
      {name: 'Ок', onClick: removeProvider},
      {name: 'Отмена'}
    ]
  });

  function removeProvider() {
    context.global.executeAction(context, {
      DeleteAction: {
        Accept: false,
        DestinationValue: {
          Source: 'ServiceProviderDataSource',
          Property: '$'
        }
      }
    }, function() {
      SchoolStore.router.goToUrl(context, '/providers', function() {
        toastr.success('Поставщик успешно удалён');
      });
    });
  }
};

//###ServiceProvider\ServiceProviderPreview\ServiceCaptionLabelTextConverter.js
SchoolStore.scripts.serviceProviderPreviewServiceCaptionLabelTextConverter = function(context, args) {
  var property = args.binding.attributes.sourceProperty;
  var index = _.last(property.split('.'));

  return (parseInt(index, 10) + 1);
};
})();