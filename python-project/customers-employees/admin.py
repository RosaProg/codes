from django.contrib import admin

from .models import CustomerEmployee, CustomerEmployeeUser, CustomerEmployeeOwner


admin.site.register(CustomerEmployee)
admin.site.register(CustomerEmployeeUser)
admin.site.register(CustomerEmployeeOwner)
