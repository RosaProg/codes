from django.db import models
from organizations.base import (OrganizationBase, OrganizationUserBase,
                                OrganizationOwnerBase)


class CustomerOrganization(OrganizationBase):
	class Meta:
		app_label = ''
		#monthly_subscription = models.IntegerField(default=1000)


class CustomerOrganizationUser(OrganizationUserBase):
	class Meta:
		app_label = ''
		#user_type = models.CharField(max_length=1, default='')


class CustomerOrganizationOwner(OrganizationOwnerBase):
	class Meta:
		app_label = ''
		pass
