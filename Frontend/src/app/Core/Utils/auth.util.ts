export function isAdminRole(role: string | null | undefined): boolean {
    return (role || '') === 'SM_ACTION';
}
