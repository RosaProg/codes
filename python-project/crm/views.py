#crm/views.py
from django.contrib.auth.decorators import login_required
from django.contrib.sessions.models import Session 
from django.shortcuts import render, redirect
from django.views.generic.edit import CreateView, UpdateView, DeleteView
from django.core.urlresolvers import reverse_lazy


from .models import *
from .forms import *


@login_required
def leads_for_user(request):
	"main page for all the leads"
	prospect = Lead.objects.filter(user=request.user, status='prospect')
	qualified = Lead.objects.filter(user=request.user, status='qualified')
	contacted = Lead.objects.filter(user=request.user, status='contacted')
	leads = Lead.objects.filter(user=request.user)
	context_dict = {'prospect':prospect, 'qualified':qualified, 
					'contacted':contacted, 'leads': leads}
	return render(request, 'main/leads.html', context_dict)



@login_required
def add_lead(request):
	"""Form to add a lead"""
	if request.method == "POST":
		form = CustomerForm(request.POST)
		if form.is_valid():
			new_lead = form.save(commit=False)
			new_lead.user = request.user 
			new_lead.save()
			return redirect(leads_for_user)
		else:
			print form.errors
	#If request is a 'GET'		
	else:
		form =CustomerForm()
	context_dict = {'form':form}	
	return render(request, 'main/add_lead.html', context_dict)


@login_required
def lead_detail(request, customer_id):
		"""Function based views are a waste of time"""
		request.session['customer'] = customer_id
		lead = Customer.objects.get(id = customer_id)
		people = People.objects.filter(company_id = customer_id)
		context_dict = {'lead':lead, 'people':people}
		return render(request, 'main/lead_detail.html', context_dict)


@login_required
def add_person(request):
	"""Form to add a contact name to a lead"""
	id = request.session['customer']
	customer = Customer.objects.get(pk=id)
	if request.method == "POST":
		form = PeopleForm(request.POST)
		if form.is_valid():
			new_person = form.save(commit=False)
			new_person.company =  customer 
			new_person.save()
			return redirect(leads_for_user)
		else:
			print form.errors
	#If request is a 'GET'		
	else:
		form = PeopleForm()
	context_dict = {'form':form}	
	return render(request, 'main/add_person.html', context_dict)


@login_required
def add_note(request):
	"""Form to add note to a lead"""
	id = request.session['customer']
	customer = Customer.objects.get(pk=id)
	if request.method == 'POST':
		form = InteractionForm(request.POST)
		if form.is_valid():
			new_note = form.save(commit=False)
			new_note.customer = customer
			new_note.save()
			return redirect(leads_for_user)
		else:
			print form.errors
	else:
		form = InteractionForm()
	context_dict = {'form':form}
	return render(request, 'main/add_note.html', context_dict)


@login_required
def edit_lead(request):
	pass	