/*
 * Leaflet.draw assumes that you have already included the Leaflet library.
 */

L.drawVersion = '0.2.4-dev';

L.drawLocal = {
	draw: {
		toolbar: {
			actions: {
				title: 'Cancel drawing',
				text: 'Отмена'
			},
			undo: {
				title: 'Delete last point drawn',
				text: 'Удалить последнюю точку'
			},
			buttons: {
				polyline: 'Draw a polyline',
				polygon: 'Создать область/многоугольник',
				rectangle: 'Draw a rectangle',
				circle: 'Draw a circle',
				marker: 'Создать метку'
			}
		},
		handlers: {
			circle: {
				tooltip: {
					start: 'Click and drag to draw circle.'
				},
				radius: 'Radius'
			},
			marker: {
				tooltip: {
					start: 'Укажите точку на карте для установки метки.'
				}
			},
			polygon: {
				tooltip: {
				    start: 'Укажите точку начала многоугольника/области на карте.',
					cont: 'Укажите точку продолжения многоугольника/области.',
					end: 'Нажмите первую точку для завершения.'
				}
			},
			polyline: {
				error: '<strong>Error:</strong> shape edges cannot cross!',
				tooltip: {
					start: 'Click to start drawing line.',
					cont: 'Click to continue drawing line.',
					end: 'Click last point to finish line.'
				}
			},
			rectangle: {
				tooltip: {
					start: 'Click and drag to draw rectangle.'
				}
			},
			simpleshape: {
				tooltip: {
					end: 'Release mouse to finish drawing.'
				}
			}
		}
	},
	edit: {
		toolbar: {
			actions: {
				save: {
					title: 'Сохранить изменения.',
					text: 'Сохранить'
				},
				cancel: {
					title: 'Отмена редактирования, вернуть все изменения.',
					text: 'Отмена'
				}
			},
			buttons: {
				edit: 'Редактировать объекты',
				editDisabled: 'No layers to edit.',
				remove: 'Удалить объекты.',
				removeDisabled: 'No layers to delete.'
			}
		},
		handlers: {
			edit: {
				tooltip: {
					text: 'Двигайте точки объектов для перемещения.',
					subtext: 'Нажмите "Отмена" для отмены редактирования.'
				}
			},
			remove: {
				tooltip: {
					text: 'Нажмите на объект для его удаления.'
				}
			}
		}
	}
};
