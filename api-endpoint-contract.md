# API Endpoint Contract: Request & Response Types

This document details the request body, query parameters, and response types for every API endpoint in the CKS platform. Types reference Zod schemas from `types.ts` where possible; new types are defined inline as needed.

---


## Provider (KaaS) Multi-Tenant API

### GET `/api/provider/tenants`
**Query Params:**
```typescript
// Not in types.ts, but implied:
const ProviderTenantsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	search: z.string().optional(),
});
type ProviderTenantsQuery = z.infer<typeof ProviderTenantsQuerySchema>;
```
**Response:**
```typescript
// Tenant type is not defined in types.ts, so define inline:
const TenantSchema = z.object({
	id: z.string(),
	name: z.string(),
	status: z.string(),
	createdAt: z.string(),
	updatedAt: z.string(),
	adminEmail: z.string().email(),
	// ...add any other fields needed for tenant listing
});
type Tenant = z.infer<typeof TenantSchema>;

const ProviderTenantsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		tenants: z.array(TenantSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ProviderTenantsResponse = z.infer<typeof ProviderTenantsResponseSchema>;
```

---

### GET `/api/provider/tenants/{tenantId}`
**Response:**
```typescript
const ProviderTenantDetailResponseSchema = ApiResponseSchema.extend({
	data: TenantSchema
});
type ProviderTenantDetailResponse = z.infer<typeof ProviderTenantDetailResponseSchema>;
```

---

### POST `/api/provider/tenants`
**Request Body:**
```typescript
const CreateTenantRequestSchema = z.object({
	name: z.string(),
	adminEmail: z.string().email(),
	// ...any additional fields for provisioning
});
type CreateTenantRequest = z.infer<typeof CreateTenantRequestSchema>;
```
**Response:**
```typescript
const CreateTenantResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		tenant: TenantSchema,
		adminUser: UserSchema,
		initialPassword: z.string(),
	})
});
type CreateTenantResponse = z.infer<typeof CreateTenantResponseSchema>;
```

---

### POST `/api/provider/tenants/{tenantId}/suspend`
**Request Body:** _none_
**Response:**
```typescript
const SuspendTenantResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type SuspendTenantResponse = z.infer<typeof SuspendTenantResponseSchema>;
```

---

### DELETE `/api/provider/tenants/{tenantId}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteTenantResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteTenantResponse = z.infer<typeof DeleteTenantResponseSchema>;
```

---

### POST `/api/provider/tenants/{tenantId}/impersonate`
**Request Body:** _none_
**Response:**
```typescript
const ImpersonateTenantResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		token: z.string(),
		expiresAt: z.number(),
	})
});
type ImpersonateTenantResponse = z.infer<typeof ImpersonateTenantResponseSchema>;
```

---


## Auth API

### POST `/api/auth/login`
**Request Body:**
```typescript
// Already in types.ts
const LoginRequestSchema = z.object({
	email: z.string().email(),
	password: z.string(),
});
type LoginRequest = z.infer<typeof LoginRequestSchema>;
```
**Response:**
```typescript
const AuthTokenSchema = z.object({
	accessToken: z.string(),
	refreshToken: z.string(),
	expiresAt: z.number(),
	tokenType: z.string(),
});
type AuthToken = z.infer<typeof AuthTokenSchema>;

const LoginResponseSchema = ApiResponseSchema.extend({ data: AuthTokenSchema });
type LoginResponse = z.infer<typeof LoginResponseSchema>;
```

---

### POST `/api/auth/logout`
**Request Body:** _none_
**Response:**
```typescript
const LogoutResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type LogoutResponse = z.infer<typeof LogoutResponseSchema>;
```

---

### POST `/api/auth/refresh`
**Request Body:**
```typescript
const RefreshTokenRequestSchema = z.object({ refreshToken: z.string() });
type RefreshTokenRequest = z.infer<typeof RefreshTokenRequestSchema>;
```
**Response:**
```typescript
const RefreshTokenResponseSchema = ApiResponseSchema.extend({ data: AuthTokenSchema });
type RefreshTokenResponse = z.infer<typeof RefreshTokenResponseSchema>;
```

---

### POST `/api/auth/change-password`
**Request Body:**
```typescript
const ChangePasswordRequestSchema = z.object({ oldPassword: z.string(), newPassword: z.string() });
type ChangePasswordRequest = z.infer<typeof ChangePasswordRequestSchema>;
```
**Response:**
```typescript
const ChangePasswordResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type ChangePasswordResponse = z.infer<typeof ChangePasswordResponseSchema>;
```

---

### GET `/api/auth/me`
**Response:**
```typescript
const MeResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type MeResponse = z.infer<typeof MeResponseSchema>;
```

---

### GET `/api/auth/sessions`
**Response:**
```typescript
const SessionSchema = z.object({
	id: z.string(),
	userId: z.string(),
	createdAt: z.string(),
	lastActiveAt: z.string(),
	ip: z.string(),
	userAgent: z.string(),
	current: z.boolean().optional(),
});
type Session = z.infer<typeof SessionSchema>;

const SessionsResponseSchema = ApiResponseSchema.extend({ data: z.array(SessionSchema) });
type SessionsResponse = z.infer<typeof SessionsResponseSchema>;
```

---

### DELETE `/api/auth/sessions/{sessionId}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteSessionResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteSessionResponse = z.infer<typeof DeleteSessionResponseSchema>;
```

---

### POST `/api/auth/oidc/login`
**Request Body:**
```typescript
const OidcLoginRequestSchema = z.object({ provider: z.string(), code: z.string() });
type OidcLoginRequest = z.infer<typeof OidcLoginRequestSchema>;
```
**Response:**
```typescript
const OidcLoginResponseSchema = ApiResponseSchema.extend({ data: AuthTokenSchema });
type OidcLoginResponse = z.infer<typeof OidcLoginResponseSchema>;
```

---

### POST `/api/auth/oidc/logout`
**Request Body:** _none_
**Response:**
```typescript
const OidcLogoutResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type OidcLogoutResponse = z.infer<typeof OidcLogoutResponseSchema>;
```

---

### GET `/api/auth/oidc/session`
**Response:**
```typescript
const OidcSessionResponseSchema = ApiResponseSchema.extend({
	data: z.object({ active: z.boolean(), provider: z.string() })
});
type OidcSessionResponse = z.infer<typeof OidcSessionResponseSchema>;
```

---


## Tenant-Scoped Users & Profile

### GET `/api/tenants/{tenantId}/users`
**Query Params:**
```typescript
const ListUsersQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	search: z.string().optional(),
	role: UserRoleSchema.optional(),
});
type ListUsersQuery = z.infer<typeof ListUsersQuerySchema>;
```
**Response:**
```typescript
const ListUsersResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		users: z.array(UserSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListUsersResponse = z.infer<typeof ListUsersResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/users/{id}`
**Response:**
```typescript
const GetUserResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type GetUserResponse = z.infer<typeof GetUserResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/users`
**Request Body:**
```typescript
const CreateUserRequestSchema = z.object({
	email: z.string().email(),
	name: z.string(),
	role: UserRoleSchema,
	avatar: z.string().optional(),
	password: z.string(),
});
type CreateUserRequest = z.infer<typeof CreateUserRequestSchema>;
```
**Response:**
```typescript
const CreateUserResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type CreateUserResponse = z.infer<typeof CreateUserResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/users/{id}`
**Request Body:**
```typescript
const UpdateUserRequestSchema = z.object({
	name: z.string().optional(),
	role: UserRoleSchema.optional(),
	avatar: z.string().optional(),
	permissions: z.array(z.string()).optional(),
});
type UpdateUserRequest = z.infer<typeof UpdateUserRequestSchema>;
```
**Response:**
```typescript
const UpdateUserResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type UpdateUserResponse = z.infer<typeof UpdateUserResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/users/{id}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteUserResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteUserResponse = z.infer<typeof DeleteUserResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/users/{id}/suspend`
**Request Body:** _none_
**Response:**
```typescript
const SuspendUserResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type SuspendUserResponse = z.infer<typeof SuspendUserResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/users/{id}/activate`
**Request Body:** _none_
**Response:**
```typescript
const ActivateUserResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type ActivateUserResponse = z.infer<typeof ActivateUserResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/users/{id}/reset-password`
**Request Body:** _none_
**Response:**
```typescript
const ResetPasswordResponseSchema = ApiResponseSchema.extend({
	data: z.object({ resetToken: z.string() })
});
type ResetPasswordResponse = z.infer<typeof ResetPasswordResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/profile`
**Response:**
```typescript
const GetProfileResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type GetProfileResponse = z.infer<typeof GetProfileResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/profile`
**Request Body:**
```typescript
const UpdateProfileRequestSchema = z.object({
	name: z.string().optional(),
	email: z.string().email().optional(),
	avatar: z.string().optional(),
});
type UpdateProfileRequest = z.infer<typeof UpdateProfileRequestSchema>;
```
**Response:**
```typescript
const UpdateProfileResponseSchema = ApiResponseSchema.extend({ data: UserSchema });
type UpdateProfileResponse = z.infer<typeof UpdateProfileResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/profile/activity`
**Response:**
```typescript
const ActivitySchema = z.object({
	id: z.string(),
	userId: z.string(),
	action: z.string(),
	resourceType: z.string(),
	resourceId: z.string(),
	details: z.record(z.string(), z.any()).optional(),
	createdAt: z.string(),
});
type Activity = z.infer<typeof ActivitySchema>;

const GetProfileActivityResponseSchema = ApiResponseSchema.extend({ data: z.array(ActivitySchema) });
type GetProfileActivityResponse = z.infer<typeof GetProfileActivityResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/profile/tokens`
**Response:**
```typescript
const ApiTokenSchema = z.object({
	id: z.string(),
	name: z.string(),
	value: z.string(),
	createdAt: z.string(),
	lastUsedAt: z.string().optional(),
	userId: z.string(),
});
type ApiToken = z.infer<typeof ApiTokenSchema>;

const GetProfileTokensResponseSchema = ApiResponseSchema.extend({ data: z.array(ApiTokenSchema) });
type GetProfileTokensResponse = z.infer<typeof GetProfileTokensResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/profile/tokens`
**Request Body:**
```typescript
const CreateApiTokenRequestSchema = z.object({ name: z.string() });
type CreateApiTokenRequest = z.infer<typeof CreateApiTokenRequestSchema>;
```
**Response:**
```typescript
const CreateApiTokenResponseSchema = ApiResponseSchema.extend({ data: ApiTokenSchema });
type CreateApiTokenResponse = z.infer<typeof CreateApiTokenResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/profile/tokens/{tokenId}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteApiTokenResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteApiTokenResponse = z.infer<typeof DeleteApiTokenResponseSchema>;
```

---

---

## Tenant-Scoped Clusters

### GET `/api/tenants/{tenantId}/clusters`
**Query Params:**
```typescript
const ListClustersQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	search: z.string().optional(),
	status: ClusterStatusSchema.optional(),
});
type ListClustersQuery = z.infer<typeof ListClustersQuerySchema>;
```
**Response:**
```typescript
const ListClustersResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		clusters: z.array(ClusterSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListClustersResponse = z.infer<typeof ListClustersResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}`
**Response:**
```typescript
const GetClusterResponseSchema = ApiResponseSchema.extend({ data: ClusterSchema });
type GetClusterResponse = z.infer<typeof GetClusterResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/clusters`
**Request Body:**
```typescript
const CreateClusterRequestSchema = z.object({
	name: z.string(),
	version: z.string(),
	nodePools: z.array(NodePoolSchema),
	networking: z.object({
		cni: z.string(),
		serviceCidr: z.string(),
		podCidr: z.string(),
	}),
	addons: z.array(z.object({ name: z.string(), version: z.string(), enabled: z.boolean() })),
	tags: z.record(z.string(), z.string()).optional(),
});
type CreateClusterRequest = z.infer<typeof CreateClusterRequestSchema>;
```
**Response:**
```typescript
const CreateClusterResponseSchema = ApiResponseSchema.extend({ data: ClusterSchema });
type CreateClusterResponse = z.infer<typeof CreateClusterResponseSchema>;
```

---

### PATCH `/api/tenants/{tenantId}/clusters/{id}`
**Request Body:**
```typescript
const UpdateClusterRequestSchema = z.object({
	name: z.string().optional(),
	version: z.string().optional(),
	nodePools: z.array(NodePoolSchema).optional(),
	addons: z.array(z.object({ name: z.string(), version: z.string(), enabled: z.boolean() })).optional(),
	tags: z.record(z.string(), z.string()).optional(),
});
type UpdateClusterRequest = z.infer<typeof UpdateClusterRequestSchema>;
```
**Response:**
```typescript
const UpdateClusterResponseSchema = ApiResponseSchema.extend({ data: ClusterSchema });
type UpdateClusterResponse = z.infer<typeof UpdateClusterResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/clusters/{id}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteClusterResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteClusterResponse = z.infer<typeof DeleteClusterResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/clusters/{id}/scale`
**Request Body:**
```typescript
const ScaleClusterRequestSchema = z.object({
	nodePools: z.array(z.object({
		id: z.string(),
		nodeCount: z.number(),
	})),
});
type ScaleClusterRequest = z.infer<typeof ScaleClusterRequestSchema>;
```
**Response:**
```typescript
const ScaleClusterResponseSchema = ApiResponseSchema.extend({ data: ClusterSchema });
type ScaleClusterResponse = z.infer<typeof ScaleClusterResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}/metrics`
**Query Params:**
```typescript
const ClusterMetricsQuerySchema = z.object({
	range: z.string().optional(),
	step: z.string().optional(),
});
type ClusterMetricsQuery = z.infer<typeof ClusterMetricsQuerySchema>;
```
**Response:**
```typescript
const ClusterMetricsSchema = z.object({
	cpu: z.array(MetricSchema),
	memory: z.array(MetricSchema),
	// ...other metrics fields
});
type ClusterMetrics = z.infer<typeof ClusterMetricsSchema>;

const GetClusterMetricsResponseSchema = ApiResponseSchema.extend({ data: ClusterMetricsSchema });
type GetClusterMetricsResponse = z.infer<typeof GetClusterMetricsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}/logs`
**Query Params:**
```typescript
const ClusterLogsQuerySchema = z.object({
	since: z.string().optional(),
	tail: z.number().optional(),
});
type ClusterLogsQuery = z.infer<typeof ClusterLogsQuerySchema>;
```
**Response:**
```typescript
const LogEntrySchema = z.object({
	timestamp: z.string(),
	message: z.string(),
	level: z.string(),
	// ...other log fields
});
type LogEntry = z.infer<typeof LogEntrySchema>;

const GetClusterLogsResponseSchema = ApiResponseSchema.extend({ data: z.array(LogEntrySchema) });
type GetClusterLogsResponse = z.infer<typeof GetClusterLogsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}/workloads`
**Query Params:**
```typescript
const ClusterWorkloadsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	type: z.string().optional(),
});
type ClusterWorkloadsQuery = z.infer<typeof ClusterWorkloadsQuerySchema>;
```
**Response:**
```typescript
const WorkloadSchema = z.object({
	id: z.string(),
	name: z.string(),
	type: z.string(),
	status: z.string(),
	// ...other workload fields
});
type Workload = z.infer<typeof WorkloadSchema>;

const GetClusterWorkloadsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		workloads: z.array(WorkloadSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type GetClusterWorkloadsResponse = z.infer<typeof GetClusterWorkloadsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}/nodepools`
**Response:**
```typescript
const GetClusterNodePoolsResponseSchema = ApiResponseSchema.extend({ data: z.array(NodePoolSchema) });
type GetClusterNodePoolsResponse = z.infer<typeof GetClusterNodePoolsResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/clusters/{id}/upgrade`
**Request Body:**
```typescript
const UpgradeClusterRequestSchema = z.object({ version: z.string() });
type UpgradeClusterRequest = z.infer<typeof UpgradeClusterRequestSchema>;
```
**Response:**
```typescript
const UpgradeClusterResponseSchema = ApiResponseSchema.extend({ data: ClusterSchema });
type UpgradeClusterResponse = z.infer<typeof UpgradeClusterResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/clusters/{id}/events`
**Query Params:**
```typescript
const ClusterEventsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type ClusterEventsQuery = z.infer<typeof ClusterEventsQuerySchema>;
```
**Response:**
```typescript
const ClusterEventSchema = z.object({
	id: z.string(),
	type: z.string(),
	message: z.string(),
	timestamp: z.string(),
	// ...other event fields
});
type ClusterEvent = z.infer<typeof ClusterEventSchema>;

const GetClusterEventsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		events: z.array(ClusterEventSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type GetClusterEventsResponse = z.infer<typeof GetClusterEventsResponseSchema>;
```

---

## Tenant-Scoped Namespaces

### GET `/api/tenants/{tenantId}/namespaces`
**Query Params:**
```typescript
const ListNamespacesQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	search: z.string().optional(),
	status: NamespaceStatusSchema.optional(),
});
type ListNamespacesQuery = z.infer<typeof ListNamespacesQuerySchema>;
```
**Response:**
```typescript
const ListNamespacesResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		namespaces: z.array(NamespaceSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListNamespacesResponse = z.infer<typeof ListNamespacesResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/namespaces/{id}`
**Response:**
```typescript
const GetNamespaceResponseSchema = ApiResponseSchema.extend({ data: NamespaceSchema });
type GetNamespaceResponse = z.infer<typeof GetNamespaceResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/namespaces`
**Request Body:**
```typescript
const CreateNamespaceRequestSchema = z.object({
	name: z.string(),
	clusterId: z.string(),
	labels: z.record(z.string(), z.string()).optional(),
	annotations: z.record(z.string(), z.string()).optional(),
	quotas: ResourceQuotaSchema.optional(),
});
type CreateNamespaceRequest = z.infer<typeof CreateNamespaceRequestSchema>;
```
**Response:**
```typescript
const CreateNamespaceResponseSchema = ApiResponseSchema.extend({ data: NamespaceSchema });
type CreateNamespaceResponse = z.infer<typeof CreateNamespaceResponseSchema>;
```

---

### PATCH `/api/tenants/{tenantId}/namespaces/{id}`
**Request Body:**
```typescript
const UpdateNamespaceRequestSchema = z.object({
	name: z.string().optional(),
	labels: z.record(z.string(), z.string()).optional(),
	annotations: z.record(z.string(), z.string()).optional(),
	quotas: ResourceQuotaSchema.optional(),
});
type UpdateNamespaceRequest = z.infer<typeof UpdateNamespaceRequestSchema>;
```
**Response:**
```typescript
const UpdateNamespaceResponseSchema = ApiResponseSchema.extend({ data: NamespaceSchema });
type UpdateNamespaceResponse = z.infer<typeof UpdateNamespaceResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/namespaces/{id}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteNamespaceResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteNamespaceResponse = z.infer<typeof DeleteNamespaceResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/namespaces/{id}/scale`
**Request Body:**
```typescript
const ScaleNamespaceRequestSchema = z.object({
	cpu: z.string().optional(),
	memory: z.string().optional(),
	pods: z.number().optional(),
});
type ScaleNamespaceRequest = z.infer<typeof ScaleNamespaceRequestSchema>;
```
**Response:**
```typescript
const ScaleNamespaceResponseSchema = ApiResponseSchema.extend({ data: NamespaceSchema });
type ScaleNamespaceResponse = z.infer<typeof ScaleNamespaceResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/namespaces/{id}/workloads`
**Query Params:**
```typescript
const NamespaceWorkloadsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	type: z.string().optional(),
});
type NamespaceWorkloadsQuery = z.infer<typeof NamespaceWorkloadsQuerySchema>;
```
**Response:**
```typescript
const GetNamespaceWorkloadsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		workloads: z.array(WorkloadSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type GetNamespaceWorkloadsResponse = z.infer<typeof GetNamespaceWorkloadsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/namespaces/{id}/quota`
**Response:**
```typescript
const GetNamespaceQuotaResponseSchema = ApiResponseSchema.extend({ data: ResourceQuotaSchema });
type GetNamespaceQuotaResponse = z.infer<typeof GetNamespaceQuotaResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/namespaces/{id}/quota`
**Request Body:**
```typescript
// ResourceQuotaSchema is used directly
```
**Response:**
```typescript
const UpdateNamespaceQuotaResponseSchema = ApiResponseSchema.extend({ data: ResourceQuotaSchema });
type UpdateNamespaceQuotaResponse = z.infer<typeof UpdateNamespaceQuotaResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/namespaces/{id}/events`
**Query Params:**
```typescript
const NamespaceEventsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type NamespaceEventsQuery = z.infer<typeof NamespaceEventsQuerySchema>;
```
**Response:**
```typescript
const NamespaceEventSchema = z.object({
	id: z.string(),
	type: z.string(),
	message: z.string(),
	timestamp: z.string(),
	// ...other event fields
});
type NamespaceEvent = z.infer<typeof NamespaceEventSchema>;

const GetNamespaceEventsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		events: z.array(NamespaceEventSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type GetNamespaceEventsResponse = z.infer<typeof GetNamespaceEventsResponseSchema>;
```

---


## Tenant-Scoped Workloads

### GET `/api/tenants/{tenantId}/workloads`
**Query Params:**
```typescript
const ListWorkloadsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	search: z.string().optional(),
	type: z.string().optional(),
	status: z.string().optional(),
});
type ListWorkloadsQuery = z.infer<typeof ListWorkloadsQuerySchema>;
```
**Response:**
```typescript
const ListWorkloadsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		workloads: z.array(WorkloadSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListWorkloadsResponse = z.infer<typeof ListWorkloadsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/workloads/{id}`
**Response:**
```typescript
const GetWorkloadResponseSchema = ApiResponseSchema.extend({ data: WorkloadSchema });
type GetWorkloadResponse = z.infer<typeof GetWorkloadResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/workloads`
**Request Body:**
```typescript
const CreateWorkloadRequestSchema = z.object({
	name: z.string(),
	namespaceId: z.string(),
	type: z.string(),
	spec: z.record(z.string(), z.any()),
});
type CreateWorkloadRequest = z.infer<typeof CreateWorkloadRequestSchema>;
```
**Response:**
```typescript
const CreateWorkloadResponseSchema = ApiResponseSchema.extend({ data: WorkloadSchema });
type CreateWorkloadResponse = z.infer<typeof CreateWorkloadResponseSchema>;
```

---

### PATCH `/api/tenants/{tenantId}/workloads/{id}`
**Request Body:**
```typescript
const UpdateWorkloadRequestSchema = z.object({
	name: z.string().optional(),
	spec: z.record(z.string(), z.any()).optional(),
});
type UpdateWorkloadRequest = z.infer<typeof UpdateWorkloadRequestSchema>;
```
**Response:**
```typescript
const UpdateWorkloadResponseSchema = ApiResponseSchema.extend({ data: WorkloadSchema });
type UpdateWorkloadResponse = z.infer<typeof UpdateWorkloadResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/workloads/{id}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteWorkloadResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteWorkloadResponse = z.infer<typeof DeleteWorkloadResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/workloads/{id}/scale`
**Request Body:**
```typescript
const ScaleWorkloadRequestSchema = z.object({ replicas: z.number() });
type ScaleWorkloadRequest = z.infer<typeof ScaleWorkloadRequestSchema>;
```
**Response:**
```typescript
const ScaleWorkloadResponseSchema = ApiResponseSchema.extend({ data: WorkloadSchema });
type ScaleWorkloadResponse = z.infer<typeof ScaleWorkloadResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/workloads/{id}/logs`
**Query Params:**
```typescript
const WorkloadLogsQuerySchema = z.object({
	since: z.string().optional(),
	tail: z.number().optional(),
});
type WorkloadLogsQuery = z.infer<typeof WorkloadLogsQuerySchema>;
```
**Response:**
```typescript
const GetWorkloadLogsResponseSchema = ApiResponseSchema.extend({ data: z.array(LogEntrySchema) });
type GetWorkloadLogsResponse = z.infer<typeof GetWorkloadLogsResponseSchema>;
```

---

## Tenant-Scoped Monitoring & Metrics

### GET `/api/tenants/{tenantId}/metrics/query`
**Query Params:**
```typescript
const MetricsQuerySchema = z.object({
	query: z.string(),
	start: z.string().optional(),
	end: z.string().optional(),
	step: z.string().optional(),
});
type MetricsQuery = z.infer<typeof MetricsQuerySchema>;
```
**Response:**
```typescript
const MetricsResultSchema = z.object({
	resultType: z.string(),
	result: z.any(), // Prometheus/Grafana result shape
});
type MetricsResult = z.infer<typeof MetricsResultSchema>;

const GetMetricsQueryResponseSchema = ApiResponseSchema.extend({ data: MetricsResultSchema });
type GetMetricsQueryResponse = z.infer<typeof GetMetricsQueryResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/monitoring/alerts`
**Query Params:**
```typescript
const ListAlertsQuerySchema = z.object({
	severity: z.string().optional(),
	status: z.string().optional(),
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type ListAlertsQuery = z.infer<typeof ListAlertsQuerySchema>;
```
**Response:**
```typescript
const ListAlertsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		alerts: z.array(AlertSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListAlertsResponse = z.infer<typeof ListAlertsResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/monitoring/alerts/{id}/acknowledge`
**Request Body:**
```typescript
const AcknowledgeAlertRequestSchema = z.object({ note: z.string().optional() });
type AcknowledgeAlertRequest = z.infer<typeof AcknowledgeAlertRequestSchema>;
```
**Response:**
```typescript
const AcknowledgeAlertResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type AcknowledgeAlertResponse = z.infer<typeof AcknowledgeAlertResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/monitoring/incidents`
**Query Params:**
```typescript
const ListIncidentsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type ListIncidentsQuery = z.infer<typeof ListIncidentsQuerySchema>;
```
**Response:**
```typescript
const IncidentSchema = z.object({
	id: z.string(),
	type: z.string(),
	status: z.string(),
	message: z.string(),
	createdAt: z.string(),
	// ...other incident fields
});
type Incident = z.infer<typeof IncidentSchema>;

const ListIncidentsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		incidents: z.array(IncidentSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListIncidentsResponse = z.infer<typeof ListIncidentsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/monitoring/health`
**Response:**
```typescript
const SystemHealthSchema = z.object({
	status: z.string(),
	details: z.record(z.string(), z.any()),
});
type SystemHealth = z.infer<typeof SystemHealthSchema>;

const GetSystemHealthResponseSchema = ApiResponseSchema.extend({ data: SystemHealthSchema });
type GetSystemHealthResponse = z.infer<typeof GetSystemHealthResponseSchema>;
```

---

## Tenant-Scoped Logs

### GET `/api/tenants/{tenantId}/logs/search`
**Query Params:**
```typescript
const SearchLogsQuerySchema = z.object({
	clusterId: z.string().optional(),
	namespaceId: z.string().optional(),
	since: z.string().optional(),
	until: z.string().optional(),
	severity: z.string().optional(),
	text: z.string().optional(),
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type SearchLogsQuery = z.infer<typeof SearchLogsQuerySchema>;
```
**Response:**
```typescript
const SearchLogsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		logs: z.array(LogEntrySchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type SearchLogsResponse = z.infer<typeof SearchLogsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/logs/{id}`
**Response:**
```typescript
const GetLogEntryResponseSchema = ApiResponseSchema.extend({ data: LogEntrySchema });
type GetLogEntryResponse = z.infer<typeof GetLogEntryResponseSchema>;
```

---

## Tenant-Scoped Billing & Usage

### GET `/api/tenants/{tenantId}/billing/usage`
**Query Params:**
```typescript
const BillingUsageQuerySchema = z.object({
	from: z.string().optional(),
	to: z.string().optional(),
	groupBy: z.string().optional(),
});
type BillingUsageQuery = z.infer<typeof BillingUsageQuerySchema>;
```
**Response:**
```typescript
const BillingUsageSummarySchema = z.object({
	// ...fields for usage summary
});
type BillingUsageSummary = z.infer<typeof BillingUsageSummarySchema>;

const GetBillingUsageResponseSchema = ApiResponseSchema.extend({ data: BillingUsageSummarySchema });
type GetBillingUsageResponse = z.infer<typeof GetBillingUsageResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/billing/invoices`
**Query Params:**
```typescript
const ListInvoicesQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type ListInvoicesQuery = z.infer<typeof ListInvoicesQuerySchema>;
```
**Response:**
```typescript
const InvoiceSchema = z.object({
	id: z.string(),
	period: z.string(),
	amount: z.number(),
	currency: z.string(),
	status: z.string(),
	issuedAt: z.string(),
	// ...other invoice fields
});
type Invoice = z.infer<typeof InvoiceSchema>;

const ListInvoicesResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		invoices: z.array(InvoiceSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListInvoicesResponse = z.infer<typeof ListInvoicesResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/billing/invoices/{id}`
**Response:**
```typescript
const GetInvoiceResponseSchema = ApiResponseSchema.extend({ data: InvoiceSchema });
type GetInvoiceResponse = z.infer<typeof GetInvoiceResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/billing/payment-methods`
**Response:**
```typescript
const PaymentMethodSchema = z.object({
	id: z.string(),
	type: z.string(),
	details: z.record(z.string(), z.any()),
	addedAt: z.string(),
	// ...other payment method fields
});
type PaymentMethod = z.infer<typeof PaymentMethodSchema>;

const GetPaymentMethodsResponseSchema = ApiResponseSchema.extend({ data: z.array(PaymentMethodSchema) });
type GetPaymentMethodsResponse = z.infer<typeof GetPaymentMethodsResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/billing/payment-methods`
**Request Body:**
```typescript
const UpdatePaymentMethodsRequestSchema = z.object({
	methods: z.array(PaymentMethodSchema),
});
type UpdatePaymentMethodsRequest = z.infer<typeof UpdatePaymentMethodsRequestSchema>;
```
**Response:**
```typescript
const UpdatePaymentMethodsResponseSchema = ApiResponseSchema.extend({ data: z.array(PaymentMethodSchema) });
type UpdatePaymentMethodsResponse = z.infer<typeof UpdatePaymentMethodsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/billing/summary`
**Response:**
```typescript
const BillingSummarySchema = z.object({
	// ...fields for billing summary
});
type BillingSummary = z.infer<typeof BillingSummarySchema>;

const GetBillingSummaryResponseSchema = ApiResponseSchema.extend({ data: BillingSummarySchema });
type GetBillingSummaryResponse = z.infer<typeof GetBillingSummaryResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/billing/cost-breakdown`
**Query Params:**
```typescript
const CostBreakdownQuerySchema = z.object({
	from: z.string().optional(),
	to: z.string().optional(),
	groupBy: z.string().optional(),
});
type CostBreakdownQuery = z.infer<typeof CostBreakdownQuerySchema>;
```
**Response:**
```typescript
const CostBreakdownSchema = z.object({
	// ...fields for cost breakdown
});
type CostBreakdown = z.infer<typeof CostBreakdownSchema>;

const GetCostBreakdownResponseSchema = ApiResponseSchema.extend({ data: CostBreakdownSchema });
type GetCostBreakdownResponse = z.infer<typeof GetCostBreakdownResponseSchema>;
```

---

## Tenant-Scoped Notifications

### GET `/api/tenants/{tenantId}/notifications`
**Query Params:**
```typescript
const ListNotificationsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	read: z.boolean().optional(),
});
type ListNotificationsQuery = z.infer<typeof ListNotificationsQuerySchema>;
```
**Response:**
```typescript
const ListNotificationsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		notifications: z.array(NotificationSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListNotificationsResponse = z.infer<typeof ListNotificationsResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/notifications/mark-read`
**Request Body:**
```typescript
const MarkNotificationsReadRequestSchema = z.object({ ids: z.array(z.string()) });
type MarkNotificationsReadRequest = z.infer<typeof MarkNotificationsReadRequestSchema>;
```
**Response:**
```typescript
const MarkNotificationsReadResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type MarkNotificationsReadResponse = z.infer<typeof MarkNotificationsReadResponseSchema>;
```

---

### DELETE `/api/tenants/{tenantId}/notifications/{id}`
**Request Body:** _none_
**Response:**
```typescript
const DeleteNotificationResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type DeleteNotificationResponse = z.infer<typeof DeleteNotificationResponseSchema>;
```

---

## Tenant-Scoped Security & Audit

### GET `/api/tenants/{tenantId}/security/audit-logs`
**Query Params:**
```typescript
const ListAuditLogsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	userId: z.string().optional(),
	action: z.string().optional(),
});
type ListAuditLogsQuery = z.infer<typeof ListAuditLogsQuerySchema>;
```
**Response:**
```typescript
const AuditLogSchema = z.object({
	id: z.string(),
	userId: z.string(),
	action: z.string(),
	resourceType: z.string(),
	resourceId: z.string(),
	details: z.record(z.string(), z.any()).optional(),
	createdAt: z.string(),
});
type AuditLog = z.infer<typeof AuditLogSchema>;

const ListAuditLogsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		logs: z.array(AuditLogSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListAuditLogsResponse = z.infer<typeof ListAuditLogsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/security/settings`
**Response:**
```typescript
const SecuritySettingsSchema = z.object({
	mfaEnabled: z.boolean(),
	passwordPolicy: z.object({ minLength: z.number(), requireNumbers: z.boolean(), requireSymbols: z.boolean() }),
	// ...other security settings fields
});
type SecuritySettings = z.infer<typeof SecuritySettingsSchema>;

const GetSecuritySettingsResponseSchema = ApiResponseSchema.extend({ data: SecuritySettingsSchema });
type GetSecuritySettingsResponse = z.infer<typeof GetSecuritySettingsResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/security/settings`
**Request Body:**
```typescript
const SecuritySettingsInputSchema = z.object({
	mfaEnabled: z.boolean().optional(),
	passwordPolicy: z.object({ minLength: z.number().optional(), requireNumbers: z.boolean().optional(), requireSymbols: z.boolean().optional() }).optional(),
	// ...other security settings fields
});
type SecuritySettingsInput = z.infer<typeof SecuritySettingsInputSchema>;
```
**Response:**
```typescript
const UpdateSecuritySettingsResponseSchema = ApiResponseSchema.extend({ data: SecuritySettingsSchema });
type UpdateSecuritySettingsResponse = z.infer<typeof UpdateSecuritySettingsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/security/events`
**Query Params:**
```typescript
const ListSecurityEventsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type ListSecurityEventsQuery = z.infer<typeof ListSecurityEventsQuerySchema>;
```
**Response:**
```typescript
const SecurityEventSchema = z.object({
	id: z.string(),
	type: z.string(),
	message: z.string(),
	timestamp: z.string(),
	// ...other event fields
});
type SecurityEvent = z.infer<typeof SecurityEventSchema>;

const ListSecurityEventsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		events: z.array(SecurityEventSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListSecurityEventsResponse = z.infer<typeof ListSecurityEventsResponseSchema>;
```

---

## Tenant-Scoped Settings & Feature Flags

### GET `/api/tenants/{tenantId}/settings`
**Response:**
```typescript
const SettingsSchema = z.object({
	// ...fields for settings
});
type Settings = z.infer<typeof SettingsSchema>;

const GetSettingsResponseSchema = ApiResponseSchema.extend({ data: SettingsSchema });
type GetSettingsResponse = z.infer<typeof GetSettingsResponseSchema>;
```

---

### PUT `/api/tenants/{tenantId}/settings`
**Request Body:**
```typescript
const SettingsInputSchema = z.object({
	// ...fields for settings input
});
type SettingsInput = z.infer<typeof SettingsInputSchema>;
```
**Response:**
```typescript
const UpdateSettingsResponseSchema = ApiResponseSchema.extend({ data: SettingsSchema });
type UpdateSettingsResponse = z.infer<typeof UpdateSettingsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/settings/feature-flags`
**Response:**
```typescript
const FeatureFlagSchema = z.object({
	key: z.string(),
	enabled: z.boolean(),
	// ...other feature flag fields
});
type FeatureFlag = z.infer<typeof FeatureFlagSchema>;

const GetFeatureFlagsResponseSchema = ApiResponseSchema.extend({ data: z.array(FeatureFlagSchema) });
type GetFeatureFlagsResponse = z.infer<typeof GetFeatureFlagsResponseSchema>;
```

---

## Tenant-Scoped Operations Center

### GET `/api/tenants/{tenantId}/operations`
**Query Params:**
```typescript
const ListOperationsQuerySchema = z.object({
	page: z.number().optional(),
	pageSize: z.number().optional(),
	type: z.string().optional(),
	status: z.string().optional(),
});
type ListOperationsQuery = z.infer<typeof ListOperationsQuerySchema>;
```
**Response:**
```typescript
const ListOperationsResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		operations: z.array(OperationSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type ListOperationsResponse = z.infer<typeof ListOperationsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/operations/{id}`
**Response:**
```typescript
const GetOperationResponseSchema = ApiResponseSchema.extend({ data: OperationSchema });
type GetOperationResponse = z.infer<typeof GetOperationResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/operations/{id}/retry`
**Request Body:** _none_
**Response:**
```typescript
const RetryOperationResponseSchema = ApiResponseSchema.extend({ data: OperationSchema });
type RetryOperationResponse = z.infer<typeof RetryOperationResponseSchema>;
```

---

## Tenant-Scoped Documentation, Tutorials, Examples

### GET `/api/tenants/{tenantId}/docs`
**Response:**
```typescript
const DocSectionSchema = z.object({
	id: z.string(),
	title: z.string(),
	content: z.string(),
	tags: z.array(z.string()),
	createdAt: z.string(),
	updatedAt: z.string(),
	createdBy: z.string(),
});
type DocSection = z.infer<typeof DocSectionSchema>;

const GetDocsResponseSchema = ApiResponseSchema.extend({ data: z.array(DocSectionSchema) });
type GetDocsResponse = z.infer<typeof GetDocsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/docs/{section}`
**Response:**
```typescript
const GetDocSectionResponseSchema = ApiResponseSchema.extend({ data: DocSectionSchema });
type GetDocSectionResponse = z.infer<typeof GetDocSectionResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/docs/tutorials`
**Response:**
```typescript
const TutorialSchema = z.object({
	id: z.string(),
	title: z.string(),
	content: z.string(),
	tags: z.array(z.string()),
	createdAt: z.string(),
	updatedAt: z.string(),
	createdBy: z.string(),
});
type Tutorial = z.infer<typeof TutorialSchema>;

const GetTutorialsResponseSchema = ApiResponseSchema.extend({ data: z.array(TutorialSchema) });
type GetTutorialsResponse = z.infer<typeof GetTutorialsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/docs/tutorials/{id}`
**Response:**
```typescript
const GetTutorialResponseSchema = ApiResponseSchema.extend({ data: TutorialSchema });
type GetTutorialResponse = z.infer<typeof GetTutorialResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/docs/examples`
**Response:**
```typescript
const ExampleSchema = z.object({
	id: z.string(),
	title: z.string(),
	code: z.string(),
	tags: z.array(z.string()),
	createdAt: z.string(),
	updatedAt: z.string(),
	createdBy: z.string(),
});
type Example = z.infer<typeof ExampleSchema>;

const GetExamplesResponseSchema = ApiResponseSchema.extend({ data: z.array(ExampleSchema) });
type GetExamplesResponse = z.infer<typeof GetExamplesResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/docs/examples/{id}`
**Response:**
```typescript
const GetExampleResponseSchema = ApiResponseSchema.extend({ data: ExampleSchema });
type GetExampleResponse = z.infer<typeof GetExampleResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/api-reference`
**Response:**
```typescript
const OpenApiSpecSchema = z.object({
	// ...OpenAPI/Swagger spec fields
});
type OpenApiSpec = z.infer<typeof OpenApiSpecSchema>;

const GetApiReferenceResponseSchema = ApiResponseSchema.extend({ data: OpenApiSpecSchema });
type GetApiReferenceResponse = z.infer<typeof GetApiReferenceResponseSchema>;
```

---

## Tenant-Scoped Onboarding & Help

### GET `/api/tenants/{tenantId}/onboarding/steps`
**Response:**
```typescript
const OnboardingStepSchema = z.object({
	id: z.string(),
	title: z.string(),
	description: z.string(),
	completed: z.boolean(),
});
type OnboardingStep = z.infer<typeof OnboardingStepSchema>;

const GetOnboardingStepsResponseSchema = ApiResponseSchema.extend({ data: z.array(OnboardingStepSchema) });
type GetOnboardingStepsResponse = z.infer<typeof GetOnboardingStepsResponseSchema>;
```

---

### POST `/api/tenants/{tenantId}/onboarding/complete`
**Request Body:** _none_
**Response:**
```typescript
const CompleteOnboardingResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type CompleteOnboardingResponse = z.infer<typeof CompleteOnboardingResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/help/links`
**Response:**
```typescript
const HelpLinkSchema = z.object({
	id: z.string(),
	label: z.string(),
	url: z.string(),
});
type HelpLink = z.infer<typeof HelpLinkSchema>;

const GetHelpLinksResponseSchema = ApiResponseSchema.extend({ data: z.array(HelpLinkSchema) });
type GetHelpLinksResponse = z.infer<typeof GetHelpLinksResponseSchema>;
```

---

## Tenant-Scoped Error Reporting

### POST `/api/tenants/{tenantId}/errors/report`
**Request Body:**
```typescript
const ErrorReportRequestSchema = z.object({
	message: z.string(),
	stack: z.string().optional(),
	context: z.record(z.string(), z.any()).optional(),
});
type ErrorReportRequest = z.infer<typeof ErrorReportRequestSchema>;
```
**Response:**
```typescript
const ErrorReportResponseSchema = ApiResponseSchema.extend({ data: z.object({}) });
type ErrorReportResponse = z.infer<typeof ErrorReportResponseSchema>;
```

---

## Tenant-Scoped Placeholder & Demo Data

### GET `/api/tenants/{tenantId}/placeholder/{w}/{h}`
**Response:**
```typescript
// Returns image/png (binary)
```

---

### GET `/api/tenants/{tenantId}/testimonials`
**Response:**
```typescript
const TestimonialSchema = z.object({
	id: z.string(),
	author: z.string(),
	content: z.string(),
	avatar: z.string().optional(),
});
type Testimonial = z.infer<typeof TestimonialSchema>;

const GetTestimonialsResponseSchema = ApiResponseSchema.extend({ data: z.array(TestimonialSchema) });
type GetTestimonialsResponse = z.infer<typeof GetTestimonialsResponseSchema>;
```

---

### GET `/api/tenants/{tenantId}/avatars/{id}`
**Response:**
```typescript
// Returns image/png (binary)
```

---

## Tenant-Scoped Search

### GET `/api/tenants/{tenantId}/search`
**Query Params:**
```typescript
const SearchQuerySchema = z.object({
	q: z.string(),
	type: z.string().optional(),
	page: z.number().optional(),
	pageSize: z.number().optional(),
});
type SearchQuery = z.infer<typeof SearchQuerySchema>;
```
**Response:**
```typescript
const SearchResultSchema = z.object({
	id: z.string(),
	type: z.string(),
	label: z.string(),
	description: z.string().optional(),
	// ...other search result fields
});
type SearchResult = z.infer<typeof SearchResultSchema>;

const SearchResponseSchema = ApiResponseSchema.extend({
	data: z.object({
		results: z.array(SearchResultSchema),
		total: z.number(),
		page: z.number(),
		pageSize: z.number(),
	})
});
type SearchResponse = z.infer<typeof SearchResponseSchema>;
```

---

## Tenant-Scoped Workloads

### `GET /api/tenants/{tenantId}/workloads`
- **Query:** `{ page?: number; pageSize?: number; search?: string; type?: string; status?: string }`
- **Response:** `ApiResponse<{ workloads: Workload[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/workloads/{id}`
- **Response:** `ApiResponse<Workload>`

### `POST /api/tenants/{tenantId}/workloads`
- **Request Body:** `WorkloadInput`
- **Response:** `ApiResponse<Workload>`

### `PATCH /api/tenants/{tenantId}/workloads/{id}`
- **Request Body:** `WorkloadInput`
- **Response:** `ApiResponse<Workload>`

### `DELETE /api/tenants/{tenantId}/workloads/{id}`
- **Response:** `ApiResponse<{}>`

### `POST /api/tenants/{tenantId}/workloads/{id}/scale`
- **Request Body:** `{ replicas: number }`
- **Response:** `ApiResponse<Workload>`

### `GET /api/tenants/{tenantId}/workloads/{id}/logs`
- **Query:** `{ since?: string; tail?: number }`
- **Response:** `ApiResponse<LogEntry[]>`

---

## Tenant-Scoped Monitoring & Metrics

### `GET /api/tenants/{tenantId}/metrics/query`
- **Query:** `{ query: string; start?: string; end?: string; step?: string }`
- **Response:** `ApiResponse<MetricsResult>`

### `GET /api/tenants/{tenantId}/monitoring/alerts`
- **Query:** `{ severity?: string; status?: string; page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ alerts: Alert[]; total: number; page: number; pageSize: number }>`

### `POST /api/tenants/{tenantId}/monitoring/alerts/{id}/acknowledge`
- **Response:** `ApiResponse<{}>`

### `GET /api/tenants/{tenantId}/monitoring/incidents`
- **Query:** `{ page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ incidents: Incident[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/monitoring/health`
- **Response:** `ApiResponse<SystemHealth>`

---

## Tenant-Scoped Logs

### `GET /api/tenants/{tenantId}/logs/search`
- **Query:** `{ clusterId?: string; namespaceId?: string; since?: string; until?: string; severity?: string; text?: string; page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ logs: LogEntry[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/logs/{id}`
- **Response:** `ApiResponse<LogEntry>`

---

## Tenant-Scoped Billing & Usage

### `GET /api/tenants/{tenantId}/billing/usage`
- **Query:** `{ from?: string; to?: string; groupBy?: string }`
- **Response:** `ApiResponse<BillingUsageSummary>`

### `GET /api/tenants/{tenantId}/billing/invoices`
- **Query:** `{ page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ invoices: Invoice[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/billing/invoices/{id}`
- **Response:** `ApiResponse<Invoice>`

### `GET /api/tenants/{tenantId}/billing/payment-methods`
- **Response:** `ApiResponse<PaymentMethod[]>`

### `PUT /api/tenants/{tenantId}/billing/payment-methods`
- **Request Body:** `PaymentMethodInput[]`
- **Response:** `ApiResponse<PaymentMethod[]>`

### `GET /api/tenants/{tenantId}/billing/summary`
- **Response:** `ApiResponse<BillingSummary>`

### `GET /api/tenants/{tenantId}/billing/cost-breakdown`
- **Query:** `{ from?: string; to?: string; groupBy?: string }`
- **Response:** `ApiResponse<CostBreakdown>`

---

## Tenant-Scoped Notifications

### `GET /api/tenants/{tenantId}/notifications`
- **Query:** `{ page?: number; pageSize?: number; read?: boolean }`
- **Response:** `ApiResponse<{ notifications: Notification[]; total: number; page: number; pageSize: number }>`

### `POST /api/tenants/{tenantId}/notifications/mark-read`
- **Request Body:** `{ ids: string[] }`
- **Response:** `ApiResponse<{}>`

### `DELETE /api/tenants/{tenantId}/notifications/{id}`
- **Response:** `ApiResponse<{}>`

---

## Tenant-Scoped Security & Audit

### `GET /api/tenants/{tenantId}/security/audit-logs`
- **Query:** `{ page?: number; pageSize?: number; userId?: string; action?: string }`
- **Response:** `ApiResponse<{ logs: AuditLog[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/security/settings`
- **Response:** `ApiResponse<SecuritySettings>`

### `PUT /api/tenants/{tenantId}/security/settings`
- **Request Body:** `SecuritySettingsInput`
- **Response:** `ApiResponse<SecuritySettings>`

### `GET /api/tenants/{tenantId}/security/events`
- **Query:** `{ page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ events: SecurityEvent[]; total: number; page: number; pageSize: number }>`

---

## Tenant-Scoped Settings & Feature Flags

### `GET /api/tenants/{tenantId}/settings`
- **Response:** `ApiResponse<Settings>`

### `PUT /api/tenants/{tenantId}/settings`
- **Request Body:** `SettingsInput`
- **Response:** `ApiResponse<Settings>`

### `GET /api/tenants/{tenantId}/settings/feature-flags`
- **Response:** `ApiResponse<FeatureFlag[]>`

---

## Tenant-Scoped Operations Center

### `GET /api/tenants/{tenantId}/operations`
- **Query:** `{ page?: number; pageSize?: number; type?: string; status?: string }`
- **Response:** `ApiResponse<{ operations: Operation[]; total: number; page: number; pageSize: number }>`

### `GET /api/tenants/{tenantId}/operations/{id}`
- **Response:** `ApiResponse<Operation>`

### `POST /api/tenants/{tenantId}/operations/{id}/retry`
- **Response:** `ApiResponse<Operation>`

---

## Tenant-Scoped Documentation, Tutorials, Examples

### `GET /api/tenants/{tenantId}/docs`
- **Response:** `ApiResponse<DocSection[]>`

### `GET /api/tenants/{tenantId}/docs/{section}`
- **Response:** `ApiResponse<DocSection>`

### `GET /api/tenants/{tenantId}/docs/tutorials`
- **Response:** `ApiResponse<Tutorial[]>`

### `GET /api/tenants/{tenantId}/docs/tutorials/{id}`
- **Response:** `ApiResponse<Tutorial>`

### `GET /api/tenants/{tenantId}/docs/examples`
- **Response:** `ApiResponse<Example[]>`

### `GET /api/tenants/{tenantId}/docs/examples/{id}`
- **Response:** `ApiResponse<Example>`

### `GET /api/tenants/{tenantId}/api-reference`
- **Response:** `ApiResponse<OpenApiSpec>`

---

## Tenant-Scoped Onboarding & Help

### `GET /api/tenants/{tenantId}/onboarding/steps`
- **Response:** `ApiResponse<OnboardingStep[]>`

### `POST /api/tenants/{tenantId}/onboarding/complete`
- **Response:** `ApiResponse<{}>`

### `GET /api/tenants/{tenantId}/help/links`
- **Response:** `ApiResponse<HelpLink[]>`

---

## Tenant-Scoped Error Reporting

### `POST /api/tenants/{tenantId}/errors/report`
- **Request Body:** `{ message: string; stack?: string; context?: any }`
- **Response:** `ApiResponse<{}>`

---

## Tenant-Scoped Placeholder & Demo Data

### `GET /api/tenants/{tenantId}/placeholder/{w}/{h}`
- **Response:** `image/png` (binary)

### `GET /api/tenants/{tenantId}/testimonials`
- **Response:** `ApiResponse<Testimonial[]>`

### `GET /api/tenants/{tenantId}/avatars/{id}`
- **Response:** `image/png` (binary)

---

## Tenant-Scoped Search

### `GET /api/tenants/{tenantId}/search`
- **Query:** `{ q: string; type?: string; page?: number; pageSize?: number }`
- **Response:** `ApiResponse<{ results: SearchResult[]; total: number; page: number; pageSize: number }>`
