import jsonpickle
class EnumHandler(jsonpickle.handlers.BaseHandler):
    def flatten(self, obj, data):
        return obj.value