import { Routes } from '@angular/router';

import { Login } from './components/login/login';
import { Dashboard } from './components/dashboard/dashboard';
import { Review } from './components/review/review';

import { authGuard } from './guards/auth-guard';

export const routes: Routes = [

  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    canActivateChild: [authGuard],

    children: [
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      },
      {
        path: 'dashboard',
        component: Dashboard
      },
      {
        path: 'review',
        component: Review
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'dashboard'
  }
];