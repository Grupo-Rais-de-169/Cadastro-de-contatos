import { CanActivateFn } from '@angular/router';

export const authGuardsGuard: CanActivateFn = (route, state) => {
  return true;
};
