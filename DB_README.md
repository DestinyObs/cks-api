# CyberCloud Kubernetes Service (CKS) — Database Schema & RBAC Logic

## Table of Contents
- [Users & Profile](#users--profile)
- [Preferences & Tokens](#preferences--tokens)
- [Clusters & Node Pools](#clusters--node-pools)
- [Namespaces](#namespaces)
- [Operations](#operations)
- [Alerts & Metrics](#alerts--metrics)
- [Billing](#billing)
- [RBAC (Role-Based Access Control)](#rbac-role-based-access-control)
- [Notifications](#notifications)
- [Documentation, Help, QA](#documentation-help-qa)
- [Activity & Audit Logs](#activity--audit-logs)
- [Enums & Reference Data](#enums--reference-data)
## Multi-Tenant Model (IMPORTANT)

- **Every table below is tenant-scoped.**
- **Every user and resource row must have a `tenant_id UUID NOT NULL` field.**
- **All queries, relationships, and RBAC assignments are tenant-scoped.**
- **No user or resource can access data from another tenant.**
- **Provider/tenant-provisioning logic is not present in this schema (FE is user/tenant-only).**

> Example: `SELECT * FROM clusters WHERE tenant_id = $currentTenantId` — always filter by tenant.

---

## Users & Profile
```sql
CREATE TABLE users (
  id UUID PRIMARY KEY,
  tenant_id UUID NOT NULL, -- <== Multi-tenant: every user belongs to a tenant
  email VARCHAR(255) UNIQUE NOT NULL,
  name VARCHAR(128) NOT NULL,
  avatar VARCHAR(255),
  role VARCHAR(32) NOT NULL, -- 'admin' | 'developer' | 'viewer'
  status VARCHAR(16) NOT NULL DEFAULT 'active', -- 'active' | 'inactive' | 'suspended'
  last_login TIMESTAMP,
  join_date TIMESTAMP NOT NULL,
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP NOT NULL
);
```

## Preferences & Tokens
```sql
CREATE TABLE user_preferences (
  user_id UUID PRIMARY KEY REFERENCES users(id) ON DELETE CASCADE,
  tenant_id UUID NOT NULL,
  notifications JSONB NOT NULL, -- { email, push, deployment, alerts, maintenance }
  dashboard JSONB NOT NULL      -- { defaultView, autoRefresh, refreshInterval }
);

CREATE TABLE user_tokens (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id) ON DELETE CASCADE,
  tenant_id UUID NOT NULL,
  name VARCHAR(64) NOT NULL,
  token VARCHAR(255) NOT NULL,
  created_at TIMESTAMP NOT NULL,
  last_used TIMESTAMP
);

CREATE TABLE user_sessions (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id) ON DELETE CASCADE,
  tenant_id UUID NOT NULL,
  device VARCHAR(128),
  ip_address VARCHAR(64),
  created_at TIMESTAMP NOT NULL,
  last_active TIMESTAMP NOT NULL
);
```

## Clusters & Node Pools
```sql
CREATE TABLE clusters (
  id UUID PRIMARY KEY,
  name VARCHAR(128) NOT NULL,
  status VARCHAR(32) NOT NULL,
  version VARCHAR(32) NOT NULL,
  networking JSONB NOT NULL,
  tags JSONB,
  tenant_id UUID NOT NULL,
  created_by UUID REFERENCES users(id),
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP NOT NULL
);

CREATE TABLE node_pools (
  id UUID PRIMARY KEY,
  cluster_id UUID REFERENCES clusters(id) ON DELETE CASCADE,
  name VARCHAR(64) NOT NULL,
  node_count INT NOT NULL,
  min_nodes INT NOT NULL,
  max_nodes INT NOT NULL,
  instance_type VARCHAR(64) NOT NULL,
  disk_size INT NOT NULL,
  labels JSONB,
  taints JSONB
);
```

## Namespaces
```sql
CREATE TABLE namespaces (
  id UUID PRIMARY KEY,
  name VARCHAR(128) NOT NULL,
  status VARCHAR(32) NOT NULL,
  cluster_id UUID REFERENCES clusters(id) ON DELETE CASCADE,
  labels JSONB,
  annotations JSONB,
  quotas JSONB,
  usage JSONB,
  tenant_id UUID NOT NULL,
  created_by UUID REFERENCES users(id),
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP NOT NULL
);
```

## Operations
```sql
CREATE TABLE operations (
  id UUID PRIMARY KEY,
  type VARCHAR(64) NOT NULL,
  status VARCHAR(32) NOT NULL,
  resource_type VARCHAR(32) NOT NULL,
  resource_id UUID NOT NULL,
  resource_name VARCHAR(128),
  progress INT NOT NULL,
  started_at TIMESTAMP NOT NULL,
  completed_at TIMESTAMP,
  error TEXT,
  tenant_id UUID NOT NULL,
  created_by UUID REFERENCES users(id)
);
```

## Alerts & Metrics
```sql
CREATE TABLE alerts (
  id UUID PRIMARY KEY,
  name VARCHAR(128) NOT NULL,
  severity VARCHAR(16) NOT NULL,
  message TEXT NOT NULL,
  source_type VARCHAR(32) NOT NULL,
  source_id UUID NOT NULL,
  source_name VARCHAR(128),
  active BOOLEAN NOT NULL,
  created_at TIMESTAMP NOT NULL,
  resolved_at TIMESTAMP,
  tenant_id UUID NOT NULL
);

-- For metrics, use a TSDB, but for schema:
CREATE TABLE metrics (
  id UUID PRIMARY KEY,
  metric JSONB NOT NULL,
  values JSONB NOT NULL
);
```

## Billing
```sql
CREATE TABLE billing_periods (
  id UUID PRIMARY KEY,
  tenant_id UUID NOT NULL,
  start_date DATE NOT NULL,
  end_date DATE NOT NULL,
  total_cost NUMERIC(12,2) NOT NULL,
  currency VARCHAR(8) NOT NULL
);

CREATE TABLE billing_usage (
  id UUID PRIMARY KEY,
  billing_period_id UUID REFERENCES billing_periods(id) ON DELETE CASCADE,
  resource_type VARCHAR(32) NOT NULL,
  amount NUMERIC(12,2) NOT NULL,
  unit VARCHAR(16) NOT NULL,
  cost NUMERIC(12,2) NOT NULL,
  currency VARCHAR(8) NOT NULL
);
```

## RBAC (Role-Based Access Control)
```sql
CREATE TABLE rbac_roles (
  id UUID PRIMARY KEY,
  name VARCHAR(64) UNIQUE NOT NULL,
  description TEXT,
  status VARCHAR(16) NOT NULL DEFAULT 'active'
);

CREATE TABLE rbac_permissions (
  id UUID PRIMARY KEY,
  name VARCHAR(64) UNIQUE NOT NULL,
  description TEXT
);

CREATE TABLE rbac_role_permissions (
  role_id UUID REFERENCES rbac_roles(id) ON DELETE CASCADE,
  permission_id UUID REFERENCES rbac_permissions(id) ON DELETE CASCADE,
  PRIMARY KEY (role_id, permission_id)
);

CREATE TABLE rbac_user_roles (
  user_id UUID REFERENCES users(id) ON DELETE CASCADE,
  role_id UUID REFERENCES rbac_roles(id) ON DELETE CASCADE,
  cluster_id UUID REFERENCES clusters(id),
  namespace_id UUID REFERENCES namespaces(id),
  PRIMARY KEY (user_id, role_id, cluster_id, namespace_id)
);
```
**RBAC Logic:**
- Each user can have multiple roles, scoped globally, per cluster, or per namespace.
- Each role maps to a set of permissions.
- Permissions are checked on every API call.
- Admins have all permissions; viewers are read-only.
- RBAC policies can be managed by security admins.

## Notifications
```sql
CREATE TABLE notifications (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id) ON DELETE CASCADE,
  type VARCHAR(32) NOT NULL,
  message TEXT NOT NULL,
  read BOOLEAN NOT NULL DEFAULT FALSE,
  created_at TIMESTAMP NOT NULL
);
```

## Documentation, Help, QA
```sql
CREATE TABLE docs (
  id UUID PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  content TEXT NOT NULL,
  category VARCHAR(64),
  tags VARCHAR(255)[],
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP NOT NULL
);

CREATE TABLE help_tickets (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id),
  subject VARCHAR(255) NOT NULL,
  description TEXT NOT NULL,
  status VARCHAR(32) NOT NULL,
  created_at TIMESTAMP NOT NULL,
  updated_at TIMESTAMP NOT NULL
);

CREATE TABLE qa_questions (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id),
  question TEXT NOT NULL,
  created_at TIMESTAMP NOT NULL
);

CREATE TABLE qa_answers (
  id UUID PRIMARY KEY,
  question_id UUID REFERENCES qa_questions(id) ON DELETE CASCADE,
  user_id UUID REFERENCES users(id),
  answer TEXT NOT NULL,
  created_at TIMESTAMP NOT NULL
);
```

## Activity & Audit Logs
```sql
CREATE TABLE activity_logs (
  id UUID PRIMARY KEY,
  user_id UUID REFERENCES users(id),
  action VARCHAR(128) NOT NULL,
  resource_type VARCHAR(64),
  resource_id UUID,
  timestamp TIMESTAMP NOT NULL,
  status VARCHAR(16)
);
```

## Enums & Reference Data
- `role`: 'admin', 'developer', 'viewer', 'namespace_admin', etc.
- `status`: 'active', 'inactive', 'suspended', 'pending', etc.
- `cluster.status`: 'pending', 'provisioning', 'running', 'upgrading', 'deleting', 'failed'
- `namespace.status`: 'active', 'terminating', 'failed'
- `operation.status`: 'pending', 'running', 'completed', 'failed', 'cancelled'
- `alert.severity`: 'info', 'warning', 'error', 'critical'

---