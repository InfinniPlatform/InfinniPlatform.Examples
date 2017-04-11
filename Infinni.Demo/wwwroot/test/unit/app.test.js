describe('getValidationErrors', function() {
  it('should return valid result', function() {
    // Given
    var fields = [
      {field: 'Test', caption: 'Test string filed'},
      {field: 1, caption: 'Test number filed'}
    ];

    // When
    var result = SchoolStore.common.getValidationErrors(fields);

    // Then
    expect(result.IsValid).to.equal(true);
    expect(result.Items).to.have.length.of(0);
  });

  it('should return invalid result', function() {
    // Given
    var fields = [
      {field: 'Test', caption: 'Test string filed'},
      {field: 1, caption: 'Test number filed'},
      {field: '', caption: 'Empty string'},
      {field: '    ', caption: 'White space'},
      {message: 'Custom message'}
    ];

    // When
    var result = SchoolStore.common.getValidationErrors(fields);

    // Then
    expect(result.IsValid).to.equal(false);
    expect(result.Items).to.deep.equal([
      {Message: 'Поле "Empty string" обязательно для заполнения.'},
      {Message: 'Поле "White space" обязательно для заполнения.'},
      {Message: 'Custom message'}
    ]);
  });
});

describe('ServiceCaptionLabel.TextConverter', function() {
  it('should return correct services captions if the tag is ServiceProviderEditView', function() {
    // Given
    var args = {
      binding: {
        attributes: {},
        getElement: function() {
          return {
            getTag: function() {
              return 'ServiceProviderEditView';
            }
          };
        }
      }
    };
    // When
    args.binding.attributes.sourceProperty = '$.Services.0';
    var result1 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.1';
    var result2 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.2';
    var result3 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    // Then
    expect(result1).to.equal('Услуга 1');
    expect(result2).to.equal('Услуга 2');
    expect(result3).to.equal('Услуга 3');
  });

  it('should return correct services captions if the tag is ServiceProviderPreview', function() {
    // Given
    var args = {
      binding: {
        attributes: {},
        getElement: function() {
          return {
            getTag: function() {
              return 'ServiceProviderPreview';
            }
          };
        }
      }
    };
    // When
    args.binding.attributes.sourceProperty = '$.Services.0';
    var result1 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.1';
    var result2 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.2';
    var result3 = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    // Then
    expect(result1).to.equal(1);
    expect(result2).to.equal(2);
    expect(result3).to.equal(3);
  });

  it('should return correct services captions if the tag is ServiceProviderPreview', function() {
    // Given
    var args = {
      binding: {
        attributes: {},
        getElement: function() {
          return {
            getTag: function() {
              return undefined;
            }
          };
        }
      }
    };
    // When
    var result = SchoolStore.common.serviceCaptionLabelTextConverter(null, args);

    // Then
    expect(result).to.equal('');
  });
});



describe('CatalogGoods.CategoryGoodsEditView.ConverterLabelText', function() {
  it('text should appear "Add category"', function() {
    // Given
    var args = {
      value: 'add'
    };
    var context = {
      dataSources: {
        CatalogGoodsDataSource: {
          getSelectedItem: function() {
            return {
              _id: 1
            };
          }
        }
      }
    };

    // When
    var result = SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewConverterLabelText(context, args);

    // Then
    assert.equal(result, 'Добавить категорию');
  });

  it('text should appear "Edit category"', function() {
    // Given
    var directoryCategories = [
      {_id: 1, Name: 'Подкатегория 1'}
    ];

    var args = {
      value: 'edit'
    };

    var context = {
      dataSources: {
        CatalogGoodsDataSource: {
          getItems: function() {
            return directoryCategories;
          },
          getSelectedItem: function() {
            return {
              ParentId: 'ROOT_CATEGORY'
            };
          }
        }
      }
    };

    // When
    var result = SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewConverterLabelText(context, args);

    // Then
    assert.equal(result, 'Редактировать категорию');
  });

  it('text should appear "Add subcategory"', function() {
    // Given
    var args = {
      value: 'add'
    };

    var context = {
      dataSources: {
        CatalogGoodsDataSource: {
          getSelectedItem: function() {
            return {
              ParentId: 1
            };
          }
        }
      }
    };

    // When
    var result = SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewConverterLabelText(context, args);

    // Then
    assert.equal(result, 'Добавить подкатегорию');
  });

  it('text should appear "Edit subcategory"', function() {
    // Given
    var directoryCategories = [
      {_id: 1, Name: 'Подкатегория 1'}
    ];
    var args = {
      value: 'edit'
    };

    var context = {
      dataSources: {
        CatalogGoodsDataSource: {
          getItems: function() {
            return directoryCategories;
          },
          getSelectedItem: function() {
            return {
              ParentId: 2
            };
          }
        }
      }
    };

    // When
    var result = SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewConverterLabelText(context, args);

    // Then
    assert.equal(result, 'Редактировать подкатегорию');
  });
});

describe('CatalogGoods.CategoryGoodsEditView.SaveButtonOnClick', function() {
  it('should create category or subcategory', function() {
    // Given
    var actionExecutedCount = 0;
    var directoryCategories = [
      {_id: '1', Name: 'Категория 1'},
      {_id: '2', Name: 'Категория 2'},
      {_id: '3', Name: 'Категория 4'}
    ];

    var context = {
      dataSources: {
        CatalogGoodsDataSource: {
          getItems: function() {
            return directoryCategories;
          }
        },
        AddCategoryDataSource: {
          createItem: function() {
            return true;
          },
          setProperty: function() {
            return true;
          }
        }
      },
      global: {
        executeAction: function(ctx, arg, callback) {
          actionExecutedCount++;
        }
      }
    };

    // When
    SchoolStore.scripts.catalogGoodsCategoryGoodsEditViewSaveButtonOnClick(context, null);

    // Then
    assert.equal(actionExecutedCount, 1);
  });
});

describe('Common.SignInView.MainDataSourceOnPropertyChanged', function() {
  var values;
  var context = {
    controls: {
      SignInButton: {
        setEnabled: function(value) {
          values.push(value);
        }
      }
    }
  };

  beforeEach(function() {
    values = [];
  });

  it('should set enabled true value', function() {
    // Given
    var args = {
      source: {
        getSelectedItem: function() {
          return {username: 'username', password: 'password'};
        }
      }
    };

    // When
    SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged(context, args);

    // Then
    assert.deepEqual(values, [true]);
  });

  it('should not set enabled true value', function() {
    // Given
    var args = {
      source: {
        getSelectedItem: function() {
          return this.data;
        },
        setData: function(data) {
          this.data = data;
        }
      }
    };

    // When
    args.source.setData();
    SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged(context, args);

    args.source.setData({});
    SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged(context, args);

    args.source.setData({username: 'username'});
    SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged(context, args);

    args.source.setData({password: 'password'});
    SchoolStore.scripts.signInViewMainDataSourceOnPropertyChanged(context, args);

    // Then
    assert.deepEqual(values, [false, false, false, false]);
  });
});

describe('ServiceProviderEditView.MainDataSource.ValidationErrors', function() {
  var context = {
    dataSources: {
      MainDataSource: {
        getProperty: function(property) {
          if (property === '$.Services') {
            return this.services;
          }
        },
        setServices: function(value) {
          this.services = value;
        }
      }
    }
  };

  it('should return valid result when all fields filled correctly', function() {
    // Given
    context.dataSources.MainDataSource.setServices([{Name: 'Услуга', Description: 'Описание'}]);
    var item = {
      Name: 'Поставщик',
      Address: 'Адрес поставщика',
      CityId: 'city-id',
      Description: 'Описание',
      Email: 'test@infinnity.ru',
      Phone: '+7',
      CategoryId: 'category-id'
    };

    // When
    var result = SchoolStore.scripts.serviceProviderEditViewMainDataSourceValidationErrors(context, item);

    // Then
    expect(result.IsValid).to.equal(true);
    expect(result.Items).to.have.length.of(0);
  });

  it('should return invalid result when not all fields filled correctly', function() {
    // Given
    context.dataSources.MainDataSource.setServices([{Name: 'Услуга', Description: 'Описание'}, {Name: 'Услуга 2'}]);
    var item = {
      Name: 'Поставщик',
      CityId: 'city-id',
      Email: 'test@infinnity.ru',
      CategoryId: 'category-id'
    };

    // When
    var result = SchoolStore.scripts.serviceProviderEditViewMainDataSourceValidationErrors(context, item);

    // Then
    expect(result.IsValid).to.equal(false);
    expect(result.Items).to.deep.equal([
      {Message: 'Поле "Адрес" обязательно для заполнения.'},
      {Message: 'Поле "Описание" обязательно для заполнения.'},
      {Message: 'Поле "Телефон" обязательно для заполнения.'},
      {Message: 'Заполнены не все услуги.'}
    ]);
  });
});

describe('ServiceProviderEditView.OnLoaded', function() {
  var context = {
    dataSources: {
      MainDataSource: {
        getSelectedItem: function() {
          return this.provider;
        },
        setProperty: function(property, value) {
          this[property] = value;
        },
        getProperty: function(property) {
          return this[property];
        },
        setProvider: function(value) {
          this.provider = value;
        }
      }
    },
    controls: {
      CityComboBox: {
        setValue: function(value) {
          this.value = value;
        },
        getValue: function() {
          return this.value;
        }
      },
      CategoryComboBox: {
        setValue: function(value) {
          this.value = value;
        },
        getValue: function() {
          return this.value;
        }
      }
    }
  };

  beforeEach(function() {
    context.controls.CategoryComboBox.setValue(null);
    context.controls.CityComboBox.setValue(null);
    context.dataSources.MainDataSource.setProperty('$.Services', null);
  });

  it('should set value to ComboBoxes when view opened as edit', function() {
    // Given
    context.dataSources.MainDataSource.setProvider({
      _header: {},
      City: 'City',
      Category: 'Category'
    });

    // When
    SchoolStore.scripts.serviceProviderEditViewOnLoaded(context, null);

    // Then
    expect(context.controls.CityComboBox.getValue()).to.equal('City');
    expect(context.controls.CategoryComboBox.getValue()).to.equal('Category');
    expect(context.dataSources.MainDataSource.getProperty('$.Services')).to.be.a('null');
  });

  it('should set property to DataSource when view opened as create', function() {
    // Given
    context.dataSources.MainDataSource.setProvider({});

    // When
    SchoolStore.scripts.serviceProviderEditViewOnLoaded(context, null);

    // Then
    expect(context.controls.CityComboBox.getValue()).to.be.a('null');
    expect(context.controls.CategoryComboBox.getValue()).to.be.a('null');
    expect(context.dataSources.MainDataSource.getProperty('$.Services')).to.deep.equal([{}]);
  });
});

describe('ServiceProviderEditView.RemoveServiceButton.OnClick', function() {
  var context = {
    dataSources: {
      MainDataSource: {
        getProperty: function(property) {
          return this[property];
        },
        setProperty: function(property, value) {
          this[property] = value;
        }
      }
    }
  };

  beforeEach(function() {
    context.dataSources.MainDataSource.setProperty('$.Services', null);
  });

  it('should remove second service', function() {
    // Given
    var services = [
      {one: 'one'},
      {two: 2},
      {three: true}
    ];
    var args = {
      source: {
        getTag: function() {
          return services[1];
        }
      }
    };
    context.dataSources.MainDataSource.setProperty('$.Services', services);

    // When
    SchoolStore.scripts.serviceProviderEditViewRemoveServiceButtonOnClick(context, args);

    var result = context.dataSources.MainDataSource.getProperty('$.Services');

    // Then
    expect(result).to.deep.equal([{one: 'one'}, {three: true}]);
  });

  it('should not remove first service', function() {
    // Given
    var services = [
      {one: 'one'},
      {two: 2},
      {three: true}
    ];
    var args = {
      source: {
        getTag: function() {
          return services[0];
        }
      }
    };
    context.dataSources.MainDataSource.setProperty('$.Services', services);

    // When
    SchoolStore.scripts.serviceProviderEditViewRemoveServiceButtonOnClick(context, args);

    var result = context.dataSources.MainDataSource.getProperty('$.Services');

    // Then
    expect(result).to.deep.equal(services);
  });
});

describe('ServiceProviderEditView.ServiceCaptionLabel.TextConverter', function() {
  var args = {
    binding: {
      attributes: {}
    }
  };

  it('should return correct services captions', function() {
    // Given

    // When
    args.binding.attributes.sourceProperty = '$.Services.0';
    var result1 = SchoolStore.scripts.serviceProviderEditViewServiceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.1';
    var result2 = SchoolStore.scripts.serviceProviderEditViewServiceCaptionLabelTextConverter(null, args);

    args.binding.attributes.sourceProperty = '$.Services.2';
    var result3 = SchoolStore.scripts.serviceProviderEditViewServiceCaptionLabelTextConverter(null, args);

    // Then
    expect(result1).to.equal('Услуга 1');
    expect(result2).to.equal('Услуга 2');
    expect(result3).to.equal('Услуга 3');
  });
});

describe('ServiceProviderEditView.SubCategoriesDataSource.OnItemsUpdated', function() {
  var context = {
    dataSources: {
      MainDataSource: {
        getProperty: function(property) {
          return this[property];
        },
        setProperty: function(property, value) {
          this[property] = value;
        }
      }
    },
    controls: {
      SubCategoriesStackPanel: {
        findAllChildrenByType: function(type) {
          return this._controls[type];
        },
        _pushControl: function(type, control) {
          this._controls = this._controls || {};
          this._controls[type] = this._controls[type] || [];
          this._controls[type].push(control);
        },
        _clearControls: function() {
          this._controls = {};
        }
      }
    }
  };

  function CheckBoxStub(tag) {
    this.getTag = function() {
      return tag;
    };

    this.setValue = function(value) {
      this._value = value;
    };

    this._value = false;
  }

  beforeEach(function() {
    context.dataSources.MainDataSource.setProperty('$.CategoryId', null);
    context.dataSources.MainDataSource.setProperty('$.SubCategories', null);
    context.controls.SubCategoriesStackPanel._clearControls();
  });

  it('should load CheckBoxes state', function() {
    // Given
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 1}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 2}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 3}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 4}));

    context.dataSources.MainDataSource.setProperty('$.CategoryId', 1);
    context.dataSources.MainDataSource.setProperty('$.SubCategories', [
      {Category: {ParentId: 1}, CategoryId: 1},
      {Category: {ParentId: 1}, CategoryId: 2},
      {Category: {ParentId: 1}, CategoryId: 3}
    ]);

    // When
    SchoolStore.scripts.serviceProviderEditViewSubCategoriesDataSourceOnItemsUpdated(context, null);

    var checkBoxes = context.controls.SubCategoriesStackPanel.findAllChildrenByType('CheckBox');

    // Then
    expect(checkBoxes[0]._value).to.equal(true);
    expect(checkBoxes[1]._value).to.equal(true);
    expect(checkBoxes[2]._value).to.equal(true);
    expect(checkBoxes[3]._value).to.equal(false);
  });

  it('should set empty array to SubCategories property', function() {
    // Given
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 1}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 2}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 3}));
    context.controls.SubCategoriesStackPanel._pushControl('CheckBox', new CheckBoxStub({_id: 4}));

    context.dataSources.MainDataSource.setProperty('$.CategoryId', 1);
    context.dataSources.MainDataSource.setProperty('$.SubCategories', [
      {Category: {ParentId: 2}, CategoryId: 1},
      {Category: {ParentId: 2}, CategoryId: 2},
      {Category: {ParentId: 2}, CategoryId: 3}
    ]);

    // When
    SchoolStore.scripts.serviceProviderEditViewSubCategoriesDataSourceOnItemsUpdated(context, null);

    var checkBoxes = context.controls.SubCategoriesStackPanel.findAllChildrenByType('CheckBox');
    var subCategories = context.dataSources.MainDataSource.getProperty('$.SubCategories');

    // Then
    expect(checkBoxes[0]._value).to.equal(false);
    expect(checkBoxes[1]._value).to.equal(false);
    expect(checkBoxes[2]._value).to.equal(false);
    expect(checkBoxes[3]._value).to.equal(false);
    expect(subCategories).to.have.length.of(0);
  });
});

describe('ServiceProviderPreviewCityComboBox.OnValueChanged', function() {
  it('if the selected city', function() {
    // Given
    var args = {
      newValue: {
        _id: 1,
        Name: 'Челябинск'
      }
    };
    var context = {
      dataSources: {
        ServiceProviderDataSource: {
          filter: null,
          setFilter: function(filter) {
            this.filter = filter;
          }
        }
      }
    };

    // When
    context.dataSources.ServiceProviderDataSource.filter = null;
    SchoolStore.scripts.serviceProviderViewCityComboBoxOnValueChanged(context, args);

    // Then
    expect(context.dataSources.ServiceProviderDataSource.filter).to.equal('eq(CityId,\'1\')');
  });

  it('If not selected the city', function() {
    // Given
    var args = {
      newValue: {}
    };
    var context = {
      dataSources: {
        ServiceProviderDataSource: {
          filter: null,
          setFilter: function(filter) {
            this.filter = filter;
          }
        }
      }
    };

    // When
    context.dataSources.ServiceProviderDataSource.filter = null;
    SchoolStore.scripts.serviceProviderViewCityComboBoxOnValueChanged(context, args);

    // Then
    expect(context.dataSources.ServiceProviderDataSource.filter).to.equal('eq(CityId,\'{0}\')');
  });
});
