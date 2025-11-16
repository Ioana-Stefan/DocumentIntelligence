import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { App } from './app/app';
import { routes } from './app/app.routes';
import { tokenInterceptor } from './app/core/interceptors/token.interceptor';

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([tokenInterceptor])),
  ]
}).catch(err => console.error(err));