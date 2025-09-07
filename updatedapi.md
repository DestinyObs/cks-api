- ## Table of Contents
	- [Provider (KaaS) Multi-Tenant API](#provider-kaas-multi-tenant-api)
---

## **Provider (KaaS) Multi-Tenant API**

- `GET /api/provider/tenants` — List all tenants (customers)
- `GET /api/provider/tenants/{tenantId}` — Get tenant details
- `POST /api/provider/tenants` — Create new tenant (provision customer org & admin)
	- Request body: `{ name, adminEmail, ... }`
	- Response: tenant info & initial admin credentials
- `POST /api/provider/tenants/{tenantId}/suspend` — Suspend tenant
- `DELETE /api/provider/tenants/{tenantId}` — Delete tenant
- `POST /api/provider/tenants/{tenantId}/impersonate` — Provider impersonates tenant admin (support)

**Note:** When a tenant is created, the system provisions the initial customer admin user for that tenant. All further user/role management is handled by the customer admin within their tenant scope.

---
- [Auth API](#auth-api)
- [Profile API](#profile-api)
- [Token & Session API](#token--session-api)
- [Preferences API](#preferences-api)
- [Notification API](#notification-api)
- [RBAC API](#rbac-api)
- [Cluster API](#cluster-api)
- [Namespace API](#namespace-api)
- [Operations API](#operations-api)
- [Alerts API](#alerts-api)
- [Metrics API](#metrics-api)
- [Billing API](#billing-api)
- [Documentation API](#documentation-api)
- [Help & Support API](#help--support-api)
- [QA API](#qa-api)
- [Audit Log API](#audit-log-api)
- [RBAC Logic & Enforcement](#rbac-logic--enforcement)
- [Error Handling](#error-handling)
- [General API Rules](#general-api-rules)

---

## **Authentication & Identity**
- `POST /api/auth/login` — User login (JWT)
- `POST /api/auth/logout` — Invalidate session/JWT
- `POST /api/auth/refresh` — Refresh JWT
- `POST /api/auth/change-password` — Change password (enforced on first login/reset)
- `GET /api/auth/me` — Get current user/session info
- `GET /api/auth/sessions` — List active sessions for user
- `DELETE /api/auth/sessions/{sessionId}` — Revoke session
- `POST /api/auth/oidc/login` — OIDC login (future Keycloak/SSO)
- `POST /api/auth/oidc/logout` — OIDC logout
- `GET /api/auth/oidc/session` — OIDC session status

---


## **Tenant-Scoped Users & Profile**

- All user and resource endpoints below are tenant-scoped:
	- Example: `/api/tenants/{tenantId}/users`, `/api/tenants/{tenantId}/clusters`, etc.

- `GET /api/tenants/{tenantId}/users` — List/search users (filter, pagination)
- `GET /api/tenants/{tenantId}/users/{id}` — Get user details
- `POST /api/tenants/{tenantId}/users` — Create user (customer admin only)
- `PUT /api/tenants/{tenantId}/users/{id}` — Edit user (roles, info)
- `DELETE /api/tenants/{tenantId}/users/{id}` — Delete user
- `POST /api/tenants/{tenantId}/users/{id}/suspend` — Suspend user
- `POST /api/tenants/{tenantId}/users/{id}/activate` — Reactivate user
- `POST /api/tenants/{tenantId}/users/{id}/reset-password` — Force password reset
- `GET /api/tenants/{tenantId}/profile` — Get own profile
- `PUT /api/tenants/{tenantId}/profile` — Update own profile (name, email, avatar)
- `GET /api/tenants/{tenantId}/profile/activity` — Recent activity (for dashboard/profile)
- `GET /api/tenants/{tenantId}/profile/tokens` — List API tokens
- `POST /api/tenants/{tenantId}/profile/tokens` — Create API token
- `DELETE /api/tenants/{tenantId}/profile/tokens/{tokenId}` — Revoke API token

---


## **Tenant-Scoped Clusters**

- `GET /api/tenants/{tenantId}/clusters` — List/search clusters (filter, pagination)
- `GET /api/tenants/{tenantId}/clusters/{id}` — Get cluster details
- `POST /api/tenants/{tenantId}/clusters` — Create cluster
- `PATCH /api/tenants/{tenantId}/clusters/{id}` — Edit cluster
- `DELETE /api/tenants/{tenantId}/clusters/{id}` — Delete cluster
- `POST /api/tenants/{tenantId}/clusters/{id}/scale` — Scale cluster
- `GET /api/tenants/{tenantId}/clusters/{id}/metrics` — Cluster metrics (health, usage)
- `GET /api/tenants/{tenantId}/clusters/{id}/logs` — Cluster logs
- `GET /api/tenants/{tenantId}/clusters/{id}/workloads` — List workloads in cluster
- `GET /api/tenants/{tenantId}/clusters/{id}/nodepools` — List node pools
- `POST /api/tenants/{tenantId}/clusters/{id}/upgrade` — Upgrade cluster
- `GET /api/tenants/{tenantId}/clusters/{id}/events` — Cluster events/history

---


## **Tenant-Scoped Namespaces**

- `GET /api/tenants/{tenantId}/namespaces` — List/search namespaces (filter, pagination)
- `GET /api/tenants/{tenantId}/namespaces/{id}` — Get namespace details
- `POST /api/tenants/{tenantId}/namespaces` — Create namespace
- `PATCH /api/tenants/{tenantId}/namespaces/{id}` — Edit namespace
- `DELETE /api/tenants/{tenantId}/namespaces/{id}` — Delete namespace
- `POST /api/tenants/{tenantId}/namespaces/{id}/scale` — Scale namespace
- `GET /api/tenants/{tenantId}/namespaces/{id}/workloads` — List workloads in namespace
- `GET /api/tenants/{tenantId}/namespaces/{id}/quota` — Get resource quota
- `PUT /api/tenants/{tenantId}/namespaces/{id}/quota` — Update resource quota
- `GET /api/tenants/{tenantId}/namespaces/{id}/events` — Namespace events/history

---


## **Tenant-Scoped Workloads (Pods, Deployments, etc.)**

- `GET /api/tenants/{tenantId}/workloads` — List/search workloads (filter, pagination)
- `GET /api/tenants/{tenantId}/workloads/{id}` — Get workload details
- `POST /api/tenants/{tenantId}/workloads` — Create workload
- `PATCH /api/tenants/{tenantId}/workloads/{id}` — Edit workload
- `DELETE /api/tenants/{tenantId}/workloads/{id}` — Delete workload
- `POST /api/tenants/{tenantId}/workloads/{id}/scale` — Scale workload
- `GET /api/tenants/{tenantId}/workloads/{id}/logs` — Get workload logs

---


## **Tenant-Scoped Monitoring & Metrics**

- `GET /api/tenants/{tenantId}/metrics/query` — Query metrics (Prometheus/Grafana integration)
- `GET /api/tenants/{tenantId}/monitoring/alerts` — List active alerts (filter by severity, status)
- `POST /api/tenants/{tenantId}/monitoring/alerts/{id}/acknowledge` — Acknowledge alert
- `GET /api/tenants/{tenantId}/monitoring/incidents` — List incidents (future/optional)
- `GET /api/tenants/{tenantId}/monitoring/health` — System health check

---


## **Tenant-Scoped Logs**

- `GET /api/tenants/{tenantId}/logs/search` — Search logs (filters: cluster, namespace, time, severity, text)
- `GET /api/tenants/{tenantId}/logs/{id}` — Get log details

---


## **Tenant-Scoped Billing & Usage**

- `GET /api/tenants/{tenantId}/billing/usage` — Usage summary (by cluster, namespace, user)
- `GET /api/tenants/{tenantId}/billing/invoices` — List invoices
- `GET /api/tenants/{tenantId}/billing/invoices/{id}` — Download invoice
- `GET /api/tenants/{tenantId}/billing/payment-methods` — List payment methods
- `PUT /api/tenants/{tenantId}/billing/payment-methods` — Update payment methods
- `GET /api/tenants/{tenantId}/billing/summary` — Dashboard summary (for charts, trends)
- `GET /api/tenants/{tenantId}/billing/cost-breakdown` — Cost breakdown by resource

---


## **Tenant-Scoped Notifications**

- `GET /api/tenants/{tenantId}/notifications` — List notifications (filter, pagination)
- `POST /api/tenants/{tenantId}/notifications/mark-read` — Mark notification(s) as read
- `DELETE /api/tenants/{tenantId}/notifications/{id}` — Dismiss notification

---


## **Tenant-Scoped Security & Audit**

- `GET /api/tenants/{tenantId}/security/audit-logs` — List audit logs (filter, pagination)
- `GET /api/tenants/{tenantId}/security/settings` — Get security settings (MFA, password policy)
- `PUT /api/tenants/{tenantId}/security/settings` — Update security settings
- `GET /api/tenants/{tenantId}/security/events` — Security events (future/optional)

---


## **Tenant-Scoped Settings & Feature Flags**

- `GET /api/tenants/{tenantId}/settings` — Get user/system settings
- `PUT /api/tenants/{tenantId}/settings` — Update user/system settings
- `GET /api/tenants/{tenantId}/settings/feature-flags` — List feature flags (for toggling features)

---


## **Tenant-Scoped Operations Center**

- `GET /api/tenants/{tenantId}/operations` — List ongoing/previous operations (deployments, upgrades, etc.)
- `GET /api/tenants/{tenantId}/operations/{id}` — Get operation details
- `POST /api/tenants/{tenantId}/operations/{id}/retry` — Retry failed operation

---


## **Tenant-Scoped Documentation, Tutorials, Examples**

- `GET /api/tenants/{tenantId}/docs` — Get documentation index/metadata
- `GET /api/tenants/{tenantId}/docs/{section}` — Get documentation section/content
- `GET /api/tenants/{tenantId}/docs/tutorials` — List tutorials
- `GET /api/tenants/{tenantId}/docs/tutorials/{id}` — Get tutorial details
- `GET /api/tenants/{tenantId}/docs/examples` — List code examples
- `GET /api/tenants/{tenantId}/docs/examples/{id}` — Get code example details
- `GET /api/tenants/{tenantId}/api-reference` — Get OpenAPI/Swagger spec

---


## **Tenant-Scoped Onboarding & Help**

- `GET /api/tenants/{tenantId}/onboarding/steps` — Onboarding steps (for modal/wizard)
- `POST /api/tenants/{tenantId}/onboarding/complete` — Mark onboarding as complete
- `GET /api/tenants/{tenantId}/help/links` — Help links (docs, support, etc.)

---


## **Tenant-Scoped Error Reporting**

- `POST /api/tenants/{tenantId}/errors/report` — Report UI/client error (for ErrorBoundary)

---


## **Tenant-Scoped Placeholder & Demo Data**

- `GET /api/tenants/{tenantId}/placeholder/{w}/{h}` — Placeholder image (for avatars, etc.)
- `GET /api/tenants/{tenantId}/testimonials` — List testimonials (for landing page, currently static)
- `GET /api/tenants/{tenantId}/avatars/{id}` — Get avatar image (for fallback/avatar generation)

---


## **Tenant-Scoped Search**

- `GET /api/tenants/{tenantId}/search` — Global search (users, clusters, namespaces, docs, etc.)