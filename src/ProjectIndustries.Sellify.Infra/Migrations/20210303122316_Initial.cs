using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ProjectIndustries.Sellify.Infra.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.EnsureSchema(
                name: "audit");

            migrationBuilder.CreateSequence(
                name: "category_hi_lo_sequence",
                schema: "public",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "customer_hi_lo_sequence",
                schema: "public",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "product_hi_lo_sequence",
                schema: "public",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "published_web_hook_hi_lo_sequence",
                schema: "public",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "web_hook_binding_hi_lo_sequence",
                schema: "public",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "asp_net_roles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    picture = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "device_codes",
                columns: table => new
                {
                    user_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    device_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_device_codes", x => x.user_code);
                });

            migrationBuilder.CreateTable(
                name: "persisted_grants",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    subject_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    session_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    client_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    creation_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    consumed_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persisted_grants", x => x.key);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_roles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "asp_net_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asp_net_user_tokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "change_set",
                schema: "audit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<Instant>(type: "timestamp", nullable: false),
                    label = table.Column<string>(type: "text", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_change_set", x => x.id);
                    table.ForeignKey(
                        name: "fk_change_set_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "store",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    domain_name = table.Column<string>(type: "text", nullable: true),
                    mode = table.Column<int>(type: "integer", nullable: false),
                    payment_gateway_configs_stripe_api_key = table.Column<string>(type: "text", nullable: true),
                    payment_gateway_configs_stripe_webhook_secret = table.Column<string>(type: "text", nullable: true),
                    skrill_email = table.Column<string>(type: "text", nullable: true),
                    skrill_secret = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    accept_credit_cards = table.Column<bool>(type: "boolean", nullable: false),
                    webhook_id = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    removed_at = table.Column<Instant>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_store", x => x.id);
                    table.ForeignKey(
                        name: "fk_store_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_store_asp_net_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_store_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "change_set_entry",
                schema: "audit",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_id = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    change_type = table.Column<int>(type: "integer", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    ChangeSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_change_set_entry", x => x.id);
                    table.ForeignKey(
                        name: "fk_change_set_entry_change_set_change_set_id",
                        column: x => x.ChangeSetId,
                        principalSchema: "audit",
                        principalTable: "change_set",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "category",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    name = table.Column<string>(type: "text", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    parent_category_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    removed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_category_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_category_category_parent_category_id",
                        column: x => x.parent_category_id,
                        principalSchema: "public",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_category_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    email = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    removed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_customer_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_customer_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "published_web_hook",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    status_description = table.Column<string>(type: "text", nullable: true),
                    listener_endpoint = table.Column<string>(type: "text", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    timestamp = table.Column<Instant>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_published_web_hook", x => x.id);
                    table.ForeignKey(
                        name: "fk_published_web_hook_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_session",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    last_activity_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: true),
                    user_agent = table.Column<string>(type: "text", nullable: false),
                    ip_address = table.Column<IPAddress>(type: "inet", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_session", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_session_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_user_session_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "web_hook_binding",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    listener_endpoint = table.Column<string>(type: "text", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_hook_binding", x => x.id);
                    table.ForeignKey(
                        name: "fk_web_hook_binding_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "web_hooks_config",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    client_secret = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_web_hooks_config", x => x.id);
                    table.ForeignKey(
                        name: "fk_web_hooks_config_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SequenceHiLo),
                    sku = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    excerpt = table.Column<string>(type: "text", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    attributes = table.Column<string>(type: "jsonb", nullable: false),
                    picture = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    removed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_product_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_product_category_category_id",
                        column: x => x.category_id,
                        principalSchema: "public",
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<long>(type: "bigint", nullable: false),
                    product_title = table.Column<string>(type: "text", nullable: false),
                    product_picture = table.Column<string>(type: "text", nullable: true),
                    product_price = table.Column<decimal>(type: "numeric", nullable: false),
                    product_quantity = table.Column<long>(type: "bigint", nullable: false),
                    invoice_email = table.Column<string>(type: "text", nullable: false),
                    metadata = table.Column<string>(type: "jsonb", nullable: false),
                    external_tx_id = table.Column<string>(type: "text", nullable: true),
                    paid_at = table.Column<Instant>(type: "timestamp", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    removed_at = table.Column<Instant>(type: "timestamp", nullable: false),
                    store_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_asp_net_users_created_by",
                        column: x => x.created_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_order_asp_net_users_updated_by",
                        column: x => x.updated_by,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "public",
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_order_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "public",
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_store_store_id",
                        column: x => x.store_id,
                        principalSchema: "public",
                        principalTable: "store",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "asp_net_role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "role_name_index",
                table: "asp_net_roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "asp_net_user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "asp_net_user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "asp_net_user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "email_index",
                table: "asp_net_users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "user_name_index",
                table: "asp_net_users",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_category_created_by",
                schema: "public",
                table: "category",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_category_parent_category_id",
                schema: "public",
                table: "category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_category_removed_at",
                schema: "public",
                table: "category",
                column: "removed_at");

            migrationBuilder.CreateIndex(
                name: "ix_category_store_id",
                schema: "public",
                table: "category",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_category_updated_by",
                schema: "public",
                table: "category",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_change_set_updated_by",
                schema: "audit",
                table: "change_set",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_change_set_entry_change_set_id",
                schema: "audit",
                table: "change_set_entry",
                column: "ChangeSetId");

            migrationBuilder.CreateIndex(
                name: "ix_customer_created_by",
                schema: "public",
                table: "customer",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_customer_removed_at",
                schema: "public",
                table: "customer",
                column: "removed_at");

            migrationBuilder.CreateIndex(
                name: "ix_customer_store_id_email",
                schema: "public",
                table: "customer",
                columns: new[] { "store_id", "email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_updated_by",
                schema: "public",
                table: "customer",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_device_codes_device_code",
                table: "device_codes",
                column: "device_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_device_codes_expiration",
                table: "device_codes",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_order_created_by",
                schema: "public",
                table: "order",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_order_customer_id",
                schema: "public",
                table: "order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_product_id",
                schema: "public",
                table: "order",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_removed_at",
                schema: "public",
                table: "order",
                column: "removed_at");

            migrationBuilder.CreateIndex(
                name: "ix_order_store_id",
                schema: "public",
                table: "order",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_updated_by",
                schema: "public",
                table: "order",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_expiration",
                table: "persisted_grants",
                column: "expiration");

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_subject_id_client_id_type",
                table: "persisted_grants",
                columns: new[] { "subject_id", "client_id", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_persisted_grants_subject_id_session_id_type",
                table: "persisted_grants",
                columns: new[] { "subject_id", "session_id", "type" });

            migrationBuilder.CreateIndex(
                name: "ix_product_category_id",
                schema: "public",
                table: "product",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_created_by",
                schema: "public",
                table: "product",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_product_removed_at",
                schema: "public",
                table: "product",
                column: "removed_at");

            migrationBuilder.CreateIndex(
                name: "ix_product_store_id",
                schema: "public",
                table: "product",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_updated_by",
                schema: "public",
                table: "product",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_published_web_hook_store_id",
                schema: "public",
                table: "published_web_hook",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_store_created_by",
                schema: "public",
                table: "store",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "ix_store_owner_id",
                schema: "public",
                table: "store",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_store_removed_at",
                schema: "public",
                table: "store",
                column: "removed_at");

            migrationBuilder.CreateIndex(
                name: "ix_store_updated_by",
                schema: "public",
                table: "store",
                column: "updated_by");

            migrationBuilder.CreateIndex(
                name: "ix_user_session_store_id",
                schema: "public",
                table: "user_session",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_session_user_id",
                schema: "public",
                table: "user_session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_web_hook_binding_store_id",
                schema: "public",
                table: "web_hook_binding",
                column: "store_id");

            migrationBuilder.CreateIndex(
                name: "ix_web_hooks_config_store_id",
                schema: "public",
                table: "web_hooks_config",
                column: "store_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "asp_net_role_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_claims");

            migrationBuilder.DropTable(
                name: "asp_net_user_logins");

            migrationBuilder.DropTable(
                name: "asp_net_user_roles");

            migrationBuilder.DropTable(
                name: "asp_net_user_tokens");

            migrationBuilder.DropTable(
                name: "change_set_entry",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "device_codes");

            migrationBuilder.DropTable(
                name: "order",
                schema: "public");

            migrationBuilder.DropTable(
                name: "persisted_grants");

            migrationBuilder.DropTable(
                name: "published_web_hook",
                schema: "public");

            migrationBuilder.DropTable(
                name: "user_session",
                schema: "public");

            migrationBuilder.DropTable(
                name: "web_hook_binding",
                schema: "public");

            migrationBuilder.DropTable(
                name: "web_hooks_config",
                schema: "public");

            migrationBuilder.DropTable(
                name: "asp_net_roles");

            migrationBuilder.DropTable(
                name: "change_set",
                schema: "audit");

            migrationBuilder.DropTable(
                name: "customer",
                schema: "public");

            migrationBuilder.DropTable(
                name: "product",
                schema: "public");

            migrationBuilder.DropTable(
                name: "category",
                schema: "public");

            migrationBuilder.DropTable(
                name: "store",
                schema: "public");

            migrationBuilder.DropTable(
                name: "asp_net_users");

            migrationBuilder.DropSequence(
                name: "category_hi_lo_sequence",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "customer_hi_lo_sequence",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "product_hi_lo_sequence",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "published_web_hook_hi_lo_sequence",
                schema: "public");

            migrationBuilder.DropSequence(
                name: "web_hook_binding_hi_lo_sequence",
                schema: "public");
        }
    }
}
