from django.contrib import admin

from .models import OrganizationEmployee, OrganizationEmployeeUser, OrganizationEmployeeOwner


admin.site.register(OrganizationEmployee)
admin.site.register(OrganizationEmployeeUser)
admin.site.register(OrganizationEmployeeOwner)
