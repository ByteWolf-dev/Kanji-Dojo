import { Routes } from '@angular/router';

import { Login } from './components/login/login';
import { Dashboard } from './components/dashboard/dashboard';
import { Review } from './components/review/review';
import { Boss } from './components/boss/boss';
import { About } from './components/about/about';

import { authGuard } from './guards/auth-guard';

export const routes: Routes = [

  {
    path: 'login',
    component: Login
  },
  {
    path: 'about',
    component: About
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
      },
      {
        path: 'boss',
        component: Boss
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'dashboard'
  }
];